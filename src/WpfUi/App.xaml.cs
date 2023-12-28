using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfUi.Solution;

namespace WpfUi;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application 
{
    public App()
    {
        Services = ConfigureServices();
    }
    public IServiceProvider Services { get; }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddTransient<SolutionViewModel>();
        return services.BuildServiceProvider();
    }
}
