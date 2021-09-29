using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;

namespace DockerAgenda.HealthChecks.HealthCheck.Extensions
{
    /// <summary>
    /// Extension responsável por configurar health check no projeto
    /// </summary>
    public static class GCInfoHealthCheckBuilderExtensions
    {
        /// <summary>
        /// Adiciona o health check no programa
        /// </summary>
        /// <param name="builder">O Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="failureStatus">O Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus que deve ser relatado quando a verificação de saúde relata uma falha. Se o valor fornecido for nulo, em seguida, Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy ser relatado.</param>
        /// <param name="tags">Uma lista de tags que podem ser usadas para filtrar as verificações de integridade.</param>
        /// <param name="thresholdInBytes">Limite máximo de consumo de memória</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddMemoryHealthCheck(
            this IHealthChecksBuilder builder,
            string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            long? thresholdInBytes = null)
        {
            // Register a check of type GCInfo.
            builder.AddCheck<MemoryHealthCheck>(
                name ?? MemoryHealthCheck.NAME, failureStatus ?? HealthStatus.Degraded, tags);

            // Configure named options to pass the threshold into the check.
            if (thresholdInBytes.HasValue)
            {
                builder.Services.Configure<MemoryCheckOptions>(name ?? MemoryHealthCheck.NAME, options => options.Threshold = thresholdInBytes.Value);
            }

            return builder;
        }
    }
}
