using Microsoft.Extensions.DependencyInjection;

namespace TruSaber.Abstractions
{
    public interface IStartupProvider
    {
        void ConfigureServices(IServiceCollection services);
    }
}