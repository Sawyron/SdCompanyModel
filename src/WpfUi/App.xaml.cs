using Infrastructure.Solution;
using Microsoft.Extensions.DependencyInjection;
using Solution.Common;
using Solution.Company;
using Solution.Production;
using Solution.Production.Conditions;
using Solution.Sales;
using Solution.Sales.Conditions;
using System.Windows;
using WpfUi.Main;
using WpfUi.Solution;
using WpfUi.SystemSettings;

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
        services.AddTransient<MainViewModel>();
        services.AddSingleton<SolutionService>();
        services.AddTransient<SettingsViewModel>();
        services.AddSingleton(_ => CreateParameterInputService());
        services.AddSingleton<SalesParametersProvider>();
        services.AddSingleton<ProductionParametersProvider>();
        services.AddSingleton<Func<SalesParameters>>(services =>
        {
            var parametersProvider = services.GetRequiredService<SalesParametersProvider>();
            return () => parametersProvider.GetSalesParameters();
        });
        services.AddSingleton<Func<ProductionParameters>>(services =>
        {
            var parametersProvider = services.GetRequiredService<ProductionParametersProvider>();
            return () => parametersProvider.GetProductionParameters();
        });
        services.AddSingleton<SalesStepResolver>();
        services.AddSingleton<ProductionStepResolver>();
        services.AddSingleton<IStepResolver, CompanyStepResolver>();
        services.AddSingleton<CompanyInitialConditionsProvider>();
        services.AddSingleton<Func<IDictionary<string, double>>>(services =>
        {
            var provider = services.GetRequiredService<CompanyInitialConditionsProvider>();
            return () => provider.GetInitialConditions();
        });
        services.AddSingleton<SolutionResolver>();
        return services.BuildServiceProvider();
    }

    private static ParameterInputService CreateParameterInputService()
    {
        var parameters = new Dictionary<string, double>
        {
            {"ui", 1000 },
            {"T2", 1 },
            {"T3", 0.4 },
            {"T4", 8 },
            {"T5", 4 },
            {"T6", 3 },
            {"T7", 0.5 },
            {"T8", 1 },
            {"K1", 8 },
            {"w1", 1000 },
            {"y3", 1 },
            {"t2", 1 },
            {"t3", 1 },
            {"t4", 8 },
            {"t5", 4 },
            {"t6", 1 },
            {"t7", 6 },
            {"K2", 4 },
            {"v1", 1000 },
            {"v3", 1000 },
            {"v6", 1000 },
            {"v10", 1000 },
            {"v11", 1000 },
            {"Beta", 1000 * 1000 },
        };
        return new ParameterInputService(parameters);
    }
}
