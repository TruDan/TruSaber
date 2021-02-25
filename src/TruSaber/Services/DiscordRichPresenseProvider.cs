using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPC;
using Microsoft.Extensions.Hosting;

namespace TruSaber.Services
{
    public interface IDiscordRichPresenseProvider : IHostedService
    {
        void SetPresence(RichPresence richPresence);
    }

    public class DiscordRichPresenseProvider : IHostedService, IDisposable, IDiscordRichPresenseProvider
    {

        private DiscordRpcClient _client;

        public DiscordRichPresenseProvider()
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new DiscordRpcClient(Globals.DiscordApplicationId);
            _client.Initialize();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken  cancellationToken)
        {
            _client.Dispose();
            _client = null;
            return Task.CompletedTask;
        }

        public void SetPresence(RichPresence richPresence)
        {
            if (richPresence == null)
            {
                _client.ClearPresence();
                return;
            }
            
            _client.SetPresence(richPresence);
        }
        
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}