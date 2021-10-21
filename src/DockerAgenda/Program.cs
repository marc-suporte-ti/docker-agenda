using Destructurama;
using Jaeger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTracing.Util;
using Serilog;
using System;
using System.Text;

namespace DockerAgenda
{
    /// <summary>
    /// Classe de execu��o inicial
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Metodo para start da aplica��o
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var serviceName = "DockerAgenda";
            var trace = new Tracer.Builder(serviceName).Build();
            GlobalTracer.Register(trace);

            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Iniciando configura��o inicial da aplica��o
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IHostBuilder</returns>
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
            .ConfigureServices(services =>
            {
                // Enables and automatically starts the instrumentation!
                services.AddOpenTracing();
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
