using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DockerAgenda.HealthChecks.Dto
{
    /// <summary>
    /// Responsável por informar situação de cada recurso validado
    /// </summary>
    public sealed class DafaultErroHealth
    {
        /// <summary>
        /// Recurso verificado
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Situação do recurso verificado
        /// </summary>
        public HealthStatus Value { get; set; }

        /// <summary>
        /// Descrição do HealthStatus
        /// </summary>
        public string ValueDescription { get; set; }
    }
}
