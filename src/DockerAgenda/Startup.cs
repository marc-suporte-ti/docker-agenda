using DockerAgenda.Data;
using DockerAgenda.Filters;
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
                            Description = "Api de agenda, gerencia seus contatos." + (description.IsDeprecated ? " Esta versão esta depreciada." : string.Empty),
                            Contact = new OpenApiContact() { Email = "marcio.almeida.rosa@gmail.com", Name = "Marcio de Almeida Rosa" },
                            License = new OpenApiLicense() { Name = "Marc Suporte Ti" }
                        });
                }

                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, "DockerAgenda.xml");

                options.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddDbContext<DockerAgendaContext>(p =>
                p.UseSqlServer(Configuration.GetSection("ConnectionStrings").Get<string>()));
        }

        /// <summary>
        /// Configuração
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agenda v1"));

            InicializarBaseDeDados(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Inicialização do DB
        /// </summary>
        /// <param name="app"></param>
        private void InicializarBaseDeDados(IApplicationBuilder app)
        {
            //using var db = new DockerAgendaContext();

            //var migracoesPendentes = db.Database.GetPendingMigrations();

            //if (migracoesPendentes.Any())
            //{
            //    db.Database.EnsureCreated();
            //    //foreach (var migracao in migracoesPendentes)
            //    //{
            //    //    Console.WriteLine($"Migração: {migracao}");
            //    //}
            //}



            //using var db = app
            //    .ApplicationServices
            //    .CreateScope()
            //    .ServiceProvider
            //    .GetRequiredService<DockerAgendaContext>();

            //if (db.Database.EnsureCreated())
            //{
            //    db.Database.
            //    //db.Departamentos.AddRange(Enumerable.Range(1, 10)
            //    //    .Select(p => new Departamento
            //    //    {
            //    //        Descricao = $"Departamento - {p}",
            //    //        Colaboradores = Enumerable.Range(1, 10)
            //    //            .Select(x => new Colaborador
            //    //            {
            //    //                Nome = $"Colaborador: {x}/{p}"
            //    //            }).ToList()
            //    //    }));

            //    //db.SaveChanges();
            //}
        }
    }
}
