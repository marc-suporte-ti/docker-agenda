using DockerAgenda.HealthChecks.HealthCheck;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DockerAgenda.HealthChecks.Services
{
    /// <summary>
    /// Responsável pelo tempo para carrgamento das dependências do projeto
    /// </summary>
    public class StartupHostedService : IHostedService
    {
        /// <summary>
        /// Tempo de espera padrão
        /// </summary>
        private readonly int _delaySeconds = 10;

        /// <summary>
        /// Log da aplicação
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Start host da aplicação
        /// </summary>
        private readonly StartupHostedServiceHealthCheck _startupHostedServiceHealthCheck;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="startupHostedServiceHealthCheck"></param>
        public StartupHostedService(
            ILogger<StartupHostedService> logger,
            StartupHostedServiceHealthCheck startupHostedServiceHealthCheck)
        {
            _logger = logger;
            _startupHostedServiceHealthCheck = startupHostedServiceHealthCheck;
        }

        /// <summary>
        /// Inicia validação
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("O serviço de inicialização em segundo plano está sendo iniciado.");

            Task.Run(async () =>
            {
                await Task.Delay(_delaySeconds * 1000);

                _startupHostedServiceHealthCheck.StartupTaskCompleted = true;

                _logger.LogInformation("O serviço de inicialização em segundo plano foi iniciado.");
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Para validação
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("O serviço de inicialização em segundo plano está parando.");

            return Task.CompletedTask;
        }
    }
}
