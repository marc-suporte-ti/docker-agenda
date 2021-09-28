using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace DockerAgenda.HealthChecks
{
    internal class PingHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Nome padrão para carregamento das dependências
        /// </summary>
        public static string Name => "ping_check";

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("asp.net-hacker.rocks");
                    if (reply.Status != IPStatus.Success)
                    {
                        return Task.FromResult(HealthCheckResult.Unhealthy("Ping não é saudável"));
                    }

                    if (reply.RoundtripTime > 100)
                    {
                        return Task.FromResult(HealthCheckResult.Degraded("Ping está degradado"));
                    }

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
