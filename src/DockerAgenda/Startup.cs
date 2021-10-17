using DockerAgenda.Data;
using DockerAgenda.Filters;
using DockerAgenda.HealthChecks;
using DockerAgenda.Interfaces;
using DockerAgenda.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DockerAgenda
{
    /// <summary>
    /// Classe de inicialização da aplicação
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Construtor da classe de inicialização da aplicação
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuração da aplicação
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configuração do serviço
        /// </summary>
        /// <param name="services">Serviço de registro da aplicação</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            var apiVersionDescriptionProvider =
                BuildServiceProvider(services).GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options =>
            {
                options.SchemaFilter<EnumSchemaFilter>();

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo()
                        {
                            Title = string.Format("Api Agenda - Ambiente[{0}]", Configuration.GetSection("Ambiente").Get<string>()),
                            Version = description.ApiVersion.ToString(),
                            Description = "Api de agenda, gerencia seus contatos." + (description.IsDeprecated ? " Esta versão esta depreciada." : string.Empty),
                            Contact = new OpenApiContact() { Email = "marcio.almeida.rosa@gmail.com", Name = "Marcio de Almeida Rosa" },
                            License = new OpenApiLicense() { Name = "Marc Suporte Ti" }
                        });
                }

                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, "DockerAgenda.xml");

                options.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddDbContext<DockerAgendaContext>(p =>
                p.UseSqlServer(Configuration.GetSection("Connection:ConnectionStrings").Get<string>()));

            services.AddScoped<IAgendaService, AgendaService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHealthChecksCustom(Convert.ToInt64(Configuration.GetSection("MemoryCheckOptions:Threshold").Value));

            services.AddHealthChecks()
                .AddDbContextCheck<DockerAgendaContext>(nameof(DockerAgendaContext));
        }

        /// <summary>
        /// Configuração
        /// </summary>
        /// <param name="app">Instância para configuração do pipeline de uma solicitação</param>
        /// <param name="env">Instância para fornecer informações sobre o ambiente de execução</param>
        /// <param name="logger">Instância para fornecer informações para o log da aplicação</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agenda v1"));

            MigracoesPendentes(app);

            //Configura o pipeline para usar o HealthChecks e determina a URL de acesso "/health" e seu tipo de saida
            app.UseHealthChecksCustom("/health", HttpStatusCode.ServiceUnavailable, HttpStatusCode.ServiceUnavailable);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Adicionando health de pronto para a aplicação
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) =>
                    {
                        logger.LogInformation("{Timestamp} Executando /health/ready: {Name} -Tags: {2} - Result - {3}", DateTime.UtcNow, check.Name, JsonConvert.SerializeObject(check.Tags), check.Tags.Contains(ConfigureHealthCheck.READY));
                        return check.Tags.Contains(ConfigureHealthCheck.READY);
                    },
                });

                // Adicionando health de ativo para a aplicação
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    // Exclude all checks and return a 200-Ok.
                    Predicate = (check) =>
                    {
                        logger.LogInformation("{Timestamp} Executando /health/live: {Name} ", DateTime.UtcNow, check.Name);
                        return false;
                    }
                });

                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Gerar ServiceProvider
        /// </summary>
        /// <param name="services">Serviço de registro da aplicação</param>
        /// <returns>ServiceProvider</returns>
        private static ServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Validando se existe pacote não aplicado no banco
        /// </summary>
        /// <param name="app">Instância para configuração do pipeline de uma solicitação</param>
        private void MigracoesPendentes(IApplicationBuilder app)
        {
            Task.Run(() =>
            {
                using var db = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<DockerAgendaContext>();

                var migracoesPendentes = db.Database.GetPendingMigrations();

                if (migracoesPendentes.Any())
                {
                    foreach (var migracao in migracoesPendentes)
                    {
                        Console.WriteLine($"Migração: {migracao}");
                    }
                    db.Database.Migrate();
                }

                // Adiantando abertura da conexão
                db.Database.GetDbConnection().Open();
                using (var cmd = db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT 1";
                    cmd.ExecuteNonQuery();
                }
            });
        }
    }
}
