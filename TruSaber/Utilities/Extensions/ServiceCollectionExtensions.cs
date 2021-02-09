using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Gui;
using RocketUI;
using RocketUI.Input;
using SharpVR;
using TruSaber.Graphics;
using TruSaber.Scenes;
using TruSaber.Services;

namespace TruSaber.Abstractions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGameConfiguration(this IServiceCollection services)
        {
            // your base DI def here
            services.AddSingleton<VrContext>(sp => VrContext.Get());
            services.AddSingleton<IVRService, VRService>();
            services.AddSingleton<SceneManager>();
            services.AddSingleton<PerspectiveCamera>();
            services.AddSingleton<InputManager>();
//            services.AddSingleton<IGuiRenderer>();
            services.AddSingleton<GuiManager>();
        }

        public static void AddPlugins(this IServiceCollection services, params string[] pluginPaths)
        {
            foreach (var pluginPath in pluginPaths)
            {
                var path = GetPath(pluginPath);
                if (!Directory.Exists(path))
                    continue;
                //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var dirCat    = new DirectoryCatalog(path);
                var importDef = BuildImportDefinition();

                try
                {
                    using var aggregateCatalog = new AggregateCatalog();
                    aggregateCatalog.Catalogs.Add(dirCat);

                    using var compositionContainer = new CompositionContainer(aggregateCatalog);
                    var       exports              = compositionContainer.GetExports(importDef);

                    foreach (var module in exports
                        .Select(x => x.Value as IStartupProvider)
                        .Where(x => x != null))
                    {
                        module.ConfigureServices(services);
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    var builder = new StringBuilder();
                    foreach (var loaderException in e.LoaderExceptions)
                    {
                        builder.AppendFormat("{0}\n", loaderException.Message);
                    }

                    throw new TypeLoadException(builder.ToString(), e);
                }
            }
        }

        private static readonly string LocalPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        private static string GetPath(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(LocalPath, path);

            if (Path.EndsInDirectorySeparator(path))
                path = Path.TrimEndingDirectorySeparator(path);

            return Path.GetFileName(path);
        }
        
        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(x => true, typeof(IStartupProvider).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }
}