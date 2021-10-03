using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DockerAgenda.HealthChecks.HealthCheck
{
    /// <summary>
    /// Responsável por fazer a validação de consumo de memória da aplicação
    /// </summary>
    public class MemoryHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Log
        /// </summary>
        private readonly ILogger<MemoryHealthCheck> _logger;

        /// <summary>
        /// Options
        /// </summary>
        private readonly IOptionsMonitor<MemoryCheckOptions> _options;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger">Log</param>
        /// <param name="options">Options</param>
        public MemoryHealthCheck(
            ILogger<MemoryHealthCheck> logger,
            IOptionsMonitor<MemoryCheckOptions> options
            )
        {
            _logger = logger;
            _options = options;
        }

        /// <summary>
        /// Nome da instalação do healthcheck quando não informado
        /// </summary>
        public static string NAME => "memory_check";

        /// <summary>
        /// Executa a checagem de consumo de memória da aplicação
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = _options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
            {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
                { "RemainingBytes", (options.Threshold - allocated) },
            };

            HealthStatus status;

            if (allocated < options.Threshold)
            {
                status = HealthStatus.Healthy;
                _logger.LogInformation("{Timestamp} Status da Memória: {Result}", DateTime.UtcNow, status);
            }
            else
            {
                status = HealthStatus.Unhealthy;
                _logger.LogError("{Timestamp} Status da Memória: {Result}", DateTime.UtcNow, status);
            }

            return Task.FromResult(new HealthCheckResult(
                status,
                description: $"Relata o status degradado se bytes alocados >= {options.Threshold} bytes.",
                exception: null,
                data: data));
        }
    }
}
