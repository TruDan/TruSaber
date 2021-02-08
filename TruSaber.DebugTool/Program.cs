using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TruSaber.Debugging;

namespace TruSaber.DebugTool
{
    public class Program
    {


        public static void Main(string[] args)
        {
            BuildDefaultHost().Build().Run();
        }

        private static IHostBuilder BuildDefaultHost()
        {
            return new HostBuilder().ConfigureServices(collection =>
                {
                    var p = new Plugin();
                    p.ConfigureServices(collection);
                })
                ;
        }
        
    }
}