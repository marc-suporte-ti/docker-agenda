namespace DockerAgenda.HealthChecks
{
    /// <summary>
    /// Configuração de consumo máximo de memória
    /// </summary>
    public class MemoryCheckOptions
    {
        /// <summary>
        /// Definição máxima de consumo de memória
        /// </summary>
        // Failure threshold (in bytes)
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}
