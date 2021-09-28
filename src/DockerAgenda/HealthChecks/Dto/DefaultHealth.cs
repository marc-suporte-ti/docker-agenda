using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DockerAgenda.HealthChecks.Dto
{
    /// <summary>
    /// Definação da resposta padrão TopDown para HealthCheck
    /// </summary>
    public sealed class DefaultHealth
    {
        /// <summary>
        /// Status da API
        /// </summary>
        public HealthStatus Status { get; set; }

        /// <summary>
        /// Descrição do HealthStatus
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Lista de checagem dos serviços
        /// </summary>
        public DafaultErroHealth[] Errors { get; set; }

        /// <summary>
        /// Descreve situação detalhada dos serviços
        /// </summary>
        public HealthReportProxy Report { get; set; }
    }
}
