using DockerAgenda.HealthChecks.Dto;
using DockerAgenda.HealthChecks.HealthCheck;
using DockerAgenda.HealthChecks.HealthCheck.Extensions;
using DockerAgenda.HealthChecks.Readiness;
using DockerAgenda.HealthChecks.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;

namespace DockerAgenda.HealthChecks
{
    /// <summary>
    /// Classe responsável por adicionar no pipeline de execução do
    /// aplicativo o HealthCheck default e sua utilização
    /// </summary>
    public static class ConfigureHealthCheck
    {
        /// <summary>
        /// Constante para definição da tag do predicaro do health
        /// </summary>
        public const string READY = "ready";

        /// <summary>
        /// Adiciona no pipeline de execução do aplicativo a utilização de :
        /// - StartupHostedServiceHealthCheck
        /// - StartupHostedServiceHealthCheck
        /// </summary>
        /// <param name="services">O Microsoft.Extensions.DependencyInjection.IServiceCollection ao qual adicionar os serviços</param>
        /// <param name="thresholdInBytes">Configura o limite máximo aceitável de consumo de memória do executável</param>
        public static IServiceCollection AddHealthChecksCustom(this IServiceCollection services, long thresholdInBytes)
        {
            //Adicionando Middleware para checar a saúde do serviço
            services.AddHostedService<StartupHostedService>();
            services.AddSingleton<StartupHostedServiceHealthCheck>();
            services.AddSingleton<PingHealthCheck>();

            services.AddHealthChecks()
                .AddMemoryHealthCheck("memory_check", thresholdInBytes: thresholdInBytes)
                .AddCheck<StartupHostedServiceHealthCheck>(StartupHostedServiceHealthCheck.NAME, failureStatus: HealthStatus.Degraded, tags: new[] { READY })
                .AddCheck<PingHealthCheck>(PingHealthCheck.NAME, failureStatus: HealthStatus.Degraded);

            services.Configure<HealthCheckPublisherOptions>(options =>
             {
                 options.Delay = TimeSpan.FromSeconds(2);
                 options.Predicate = (check) => check.Tags.Contains(READY);
             });

            services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();

            return services;
        }

        /// <summary>
        /// Configura o pipeline para usar o HealthChecks e determina a URL de acesso "/health" e seu tipo de saida
        /// </summary>
        /// <param name="app">O Microsoft.AspNetCore.Builder.IApplicationBuilder</param>
        /// <param name="pathRootHealthCheck">Define o caminho padrão para acesso as informações de HealthCheck. (/health) é default</param>
        /// <param name="unhealthyCode">Esta propriedade pode ser usada para configurar os códigos de status retornados para cada status. Status (HealthStatus.Unhealthy). (503 - ServiceUnavailable) é dafault. </param>
        /// <param name="degradedCode">Esta propriedade pode ser usada para configurar os códigos de status retornados para cada status. Status (HealthStatus.Degraded). (503 - ServiceUnavailable) é dafault.</param>
        public static IApplicationBuilder UseHealthChecksCustom(
            this IApplicationBuilder app,
            string pathRootHealthCheck = "/health",
            HttpStatusCode unhealthyCode = HttpStatusCode.ServiceUnavailable,
            HttpStatusCode degradedCode = HttpStatusCode.ServiceUnavailable)
        {
            var options = new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResponseWriter = async (context, report) =>
                {
                    var result = new DefaultHealth
                    {
                        Status = report.Status,
                        StatusDescription = Enum.GetName(typeof(HealthStatus), report.Status),
                        Errors = report.Entries.Select(e =>
                                  new DafaultErroHealth
                                  {
                                      Key = e.Key,
                                      Value = e.Value.Status,
                                      ValueDescription = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                  }).ToArray(),
                        Report = new HealthReportProxy { Entries = report.Entries, Status = report.Status, TotalDuration = report.TotalDuration },
                    };
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }
            };

            options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)unhealthyCode;
            options.ResultStatusCodes[HealthStatus.Degraded] = (int)degradedCode;

            //Adicionando endpoint para Middleware checar a saúde do serviço
            app.UseHealthChecks(pathRootHealthCheck, options);

            return app;
        }
    }
}
