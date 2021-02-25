using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;

namespace TruSaber.DebugTool
{
    public class DebugFormsHost : IHostedService
    {
        private readonly MainForm _mainForm;
        private Thread _thread;
        
        public DebugFormsHost(MainForm mainForm)
        {
            _mainForm = mainForm;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _thread = new Thread(Run)
            {
                Name = nameof(DebugFormsHost)
            };
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
            return Task.CompletedTask;
        }

        private void Run()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(_mainForm);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Application.Exit(new CancelEventArgs());
            _thread.Join();
            return Task.CompletedTask;
        }
    }
}