using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace DockerAgenda.HealthChecks
{
    /// <summary>
    /// HealthCheck da aplicação
    /// </summary>
    public class StartupHostedServiceHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Controle de início da aplicação
        /// </summary>
        private volatile bool _startupTaskCompleted;

        /// <summary>
        /// Nome padrão para carregamento das dependências
        /// </summary>
        public static string Name => "slow_dependency_check";

        /// <summary>
        /// Iniio completo
        /// </summary>
        public bool StartupTaskCompleted
        {
            get => _startupTaskCompleted;
            set => _startupTaskCompleted = value;
        }

        /// <summary>
        /// Valida carregamento de dependências
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (StartupTaskCompleted)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A tarefa de inicialização foi concluída."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("A tarefa de inicialização ainda está em execução."));
        }
    }
}
