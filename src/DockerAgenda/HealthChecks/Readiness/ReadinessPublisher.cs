using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DockerAgenda.HealthChecks.Readiness
{
    /// <summary>
    /// Classe responsável por implementar regra de Readiness
    /// </summary>
    internal class ReadinessPublisher : IHealthCheckPublisher
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor da classe responsável por implementar regra de Readiness
        /// </summary>
        /// <param name="logger">Instância de log da aplicação</param>
        public ReadinessPublisher(ILogger<ReadinessPublisher> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executa processamento do Readiness
        /// </summary>
        /// <param name="report">Reporte do health</param>
        /// <param name="cancellationToken">Tokend de cancelamento</param>
        /// <returns>Task</returns>
        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            if (report.Status == HealthStatus.Healthy)
            {
                _logger.LogInformation("{Timestamp} Status da Sondagem de Prontidão: {Result}", DateTime.UtcNow, report.Status);
            }
            else
            {
                _logger.LogError("{Timestamp} Status da Sondagem de Prontidão: {Result}", DateTime.UtcNow, report.Status);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}
