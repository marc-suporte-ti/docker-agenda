using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace DockerAgenda.HealthChecks.HealthCheck
{
    internal class PingHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Log
        /// </summary>
        private readonly ILogger<PingHealthCheck> _logger;

        /// <summary>
        /// Nome padrão para carregamento das dependências
        /// </summary>
        public static string NAME => "ping_check";

        /// <summary>
        /// Health ping
        /// </summary>
        /// <param name="logger">Log</param>
        public PingHealthCheck(ILogger<PingHealthCheck> logger)
        {
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("asp.net-hacker.rocks");
                    if (reply.Status != IPStatus.Success)
                    {
                        _logger.LogError("{Timestamp} Status do ping: {Result}", DateTime.UtcNow, reply.Status);
                        return Task.FromResult(HealthCheckResult.Unhealthy("Ping não é saudável"));
                    }

                    if (reply.RoundtripTime > 100)
                    {
                        _logger.LogWarning("{Timestamp} Status do ping: {Result} - Roundtrip Time: {RoundtripTime}", DateTime.UtcNow, reply.Status, reply.RoundtripTime);
                        return Task.FromResult(HealthCheckResult.Degraded("Ping está degradado"));
                    }

                    _logger.LogInformation("{Timestamp} Status do ping: {Result} - Roundtrip Time: {RoundtripTime}", DateTime.UtcNow, reply.Status, reply.RoundtripTime);
                    return Task.FromResult(HealthCheckResult.Healthy("Ping é saudável"));
                }
            }
            catch
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Ping não é saudável"));
            }
        }
    }
}
