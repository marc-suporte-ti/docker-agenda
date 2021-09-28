using DockerAgenda.Data;
using DockerAgenda.Filters;
using DockerAgenda.HealthChecks;
using DockerAgenda.Interfaces;
using DockerAgenda.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DockerAgenda
{
    /// <summary>
    /// Classe de inicializa��o da aplica��o
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Construtor da classe de inicializa��o da aplica��o
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configura��o da aplica��o
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configura��o do servi�o
        /// </summary>
        /// <param name="services">Servi�o de registro da aplica��o</param>
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
                            Description = "Api de agenda, gerencia seus contatos." + (description.IsDeprecated ? " Esta vers�o esta depreciada." : string.Empty),
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
        }

        /// <summary>
        /// Configura��o
        /// </summary>
        /// <param name="app">Inst�ncia para configura��o do pipeline de uma solicita��o</param>
        /// <param name="env">Inst�ncia para fornecer informa��es sobre o ambiente de execu��o</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Gerar ServiceProvider
        /// </summary>
        /// <param name="services">Servi�o de registro da aplica��o</param>
        /// <returns>ServiceProvider</returns>
        private static ServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Validando se existe pacote n�o aplicado no banco
        /// </summary>
        /// <param name="app">Inst�ncia para configura��o do pipeline de uma solicita��o</param>
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
                        Console.WriteLine($"Migra��o: {migracao}");
                    }
                    db.Database.Migrate();
                }

                // Adiantando abertura da conex�o
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
