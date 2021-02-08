using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using TruSaber.Abstractions;
using TruSaber.Debugging;

namespace TruSaber
{
    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddEnvironmentVariables("TruSaber_");
                    builder.AddJsonFile("TruSaber.hostconfig.json", optional: true, reloadOnChange: false);
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddEnvironmentVariables("TruSaber_");
                    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddNLog()
                        .AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IGame, TruSaberGame>();
                    services.AddHostedService<Worker>();
                    services.AddGameConfiguration();

                    var pluginDirs = hostContext.Configuration.GetValue<List<string>>("PluginPaths");
                    if (pluginDirs == null)
                        pluginDirs = new List<string>(); 
                    
                    if(!pluginDirs.Any())
                        pluginDirs.Add("./Plugins");
                    
                    services.AddPlugins(pluginDirs.ToArray());
                    services.TryAddSingleton<IDebugService, NoopDebugService>();
                });
        
        private static void ConfigureNLog(string baseDir)
        {
            string loggerConfigFile = Path.Combine(baseDir, "NLog.config");
         
            string logsDir = Path.Combine(baseDir, "logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            LogManager.ThrowConfigExceptions = false;
            LogManager.LoadConfiguration(loggerConfigFile);
//			LogManager.Configuration = new XmlLoggingConfiguration(loggerConfigFile);
            LogManager.Configuration.Variables["basedir"] = baseDir;

        }
    }
}