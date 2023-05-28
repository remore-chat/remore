using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using Remore.Desktop.ViewModels;
using Remore.Desktop.Views;

namespace Remore.Desktop
{
    public partial class App : Application
    {
        private static IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {

            })
            .Build();

        public static T GetService<T>()
           where T : class
           => _host.Services.GetService(typeof(T)) as T;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}