using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TruSaber.Abstractions;
using TruSaber.Debugging;

namespace TruSaber.DebugTool
{
    public class Plugin : IStartupProvider
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDebugService, DebugService>();
            services.AddSingleton<MainForm>();
            services.AddHostedService<DebugFormsHost>();
        }
    }
}