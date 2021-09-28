using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace DockerAgenda.HealthChecks.Dto
{
    /// <summary>
    /// Proxy for  represents the result of executing a group of Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck instances.
    /// </summary>
    /// <remarks>proxy da classe Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport</remarks>
    public class HealthReportProxy
    {
        /// <summary>
        ///  A System.Collections.Generic.IReadOnlyDictionary`2 containing the results from each health check.
        /// </summary>
        /// <remarks>
        /// The keys in this dictionary map the name of each executed health check to a Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry
        /// for the result data returned from the corresponding health check.
        /// </remarks>
        public IReadOnlyDictionary<string, HealthReportEntry> Entries { get; set; }

        /// <summary>
        /// Gets a Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus representing
        /// the aggregate status of all the health checks. The value of Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport.Status
        /// will be the most severe status reported by a health check. If no checks were
        /// executed, the value is always Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy.
        /// </summary>
        public HealthStatus Status { get; set; }

        /// <summary>
        /// Gets the time the health check service took to execute.
        /// </summary>
        public TimeSpan TotalDuration { get; set; }
    }
}
