using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Text;

namespace DockerAgenda
{
    /// <summary>
    /// Classe de execução inicial
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Metodo para start da aplicação
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Iniciando configuração inicial da aplicação
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .UseSerilog((provider, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(provider.Configuration);
                loggerConfiguration.Destructure.UsingAttributes();
                loggerConfiguration.Enrich.WithEnvironmentUserName();
                loggerConfiguration.Enrich.WithMachineName();
                loggerConfiguration.Enrich.WithThreadId();
                loggerConfiguration.Enrich.WithThreadName();
                loggerConfiguration.Enrich.FromLogContext();
                Console.OutputEncoding = Encoding.UTF8;
                loggerConfiguration.WriteTo.Console();
            })
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
