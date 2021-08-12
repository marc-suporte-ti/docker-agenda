using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DockerAgenda
{
    /// <summary>
    /// Classe de execu��o inicial
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Metodo para start da aplica��o
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Iniciando configura��o inicial da aplica��o
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
