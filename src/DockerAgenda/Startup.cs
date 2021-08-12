using DockerAgenda.Data;
using DockerAgenda.Filters;
using DockerAgenda.Interfaces;
using DockerAgenda.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

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
                services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options =>
            {
                options.SchemaFilter<EnumSchemaFilter>();

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo()
                        {
                            Title = "Api Agenda",
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
                p.UseSqlServer(Configuration.GetSection("ConnectionStrings").Get<string>()));

            services.AddScoped<IAgendaService, AgendaService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Validando se existe pacote n�o aplicado no banco
        /// </summary>
        /// <param name="app">Inst�ncia para configura��o do pipeline de uma solicita��o</param>
        private void MigracoesPendentes(IApplicationBuilder app)
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
        }
    }
}
