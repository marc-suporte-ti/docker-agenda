using Microsoft.AspNetCore.Mvc;
using System;

namespace DockerAgenda.Controllers
{
    /// <summary>
    /// Controle base
    /// </summary>
    public abstract class Base : ControllerBase
    {
        /// <summary>
        /// Retorna informações da máquina no Header da resposta
        /// </summary>
        protected void RetornaHeadersPadrao()
        {
            Response.Headers.Add("Machine", Environment.MachineName);
            Response.Headers.Add("OSVersion", Environment.OSVersion.VersionString);
            Response.Headers.Add("Platform", Environment.OSVersion.Platform.ToString());
            Response.Headers.Add("ProcessId", Environment.ProcessId.ToString());
        }
    }
}
