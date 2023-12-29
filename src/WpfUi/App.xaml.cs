using Infrastructure.Solution;
using Microsoft.Extensions.DependencyInjection;
using Solution.Conditions;
using Solution.SolutionProviders;
using System.Windows;
using WpfUi.Main;
using WpfUi.Solution;
using WpfUi.SystemSettings;
using WpfUi.SystemSettings.Production;

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
        services.AddTransient<ProductionSettingsViewModel>();
        services.AddSingleton<Func<double, SystemSolutionProvider>>((_) => (interval) => CreateSolutionProvider(interval));
        services.AddSingleton<SolutionService>();
        services.AddSingleton((_) =>
            new SystemParametersService(new ProductionInput(
                Y3: 1000,
                V1: 10,
                V6: 10,
                V10: 10,
                V11: 10,
                T2: 1,
                T3: 1,
                T4: 8,
                T5: 4,
                T6: 1,
                T7: 6,
                K: 4,
                Beta: 1000 * 1000)));
        return services.BuildServiceProvider();
    }

    private static SystemSolutionProvider CreateSolutionProvider(double interval)
    {
        double ui = 1000;
        double productionT2 = 1;
        double productionT3 = 1;
        double y1 = (productionT2 + productionT3) * ui;
        var productionParameters = new ProductionParameters(
            Y1: y1,
            T2: productionT2,
            T3: productionT3,
            T4: 8,
            T5: 4,
            T6: 1,
            T7: 6,
            K: 4,
            Beta: 1000 * ui);
        var productionVariables = ProductionVariables.CreateFromInitial(
            w11: ui,
            y3: ui,
            v1: 10000,
            v6: 10000,
            v10: 10000,
            v11: 10000,
            interval: interval,
            parameters: productionParameters);
        var salesParameters = new SalesParameters(
            Demands: ui,
            OrderFulfillmentDelay: 1,
            AbsenceDelay: 0.4,
            K: 8,
            AveragingDelay: 8,
            StockControlDelay: 4,
            OrderProcessDelay: 3,
            LinkDelay: 0.5,
            ShipmentDelay: 1);
        SalesVariables salesVariables = SalesVariables.CreateFromInitial(
            x3: salesParameters.Demands,
            w1: 1000,
            y1: y1,
            v3: 1000,
            parameters: salesParameters);
        var solutionProvider = new SalesSolutionProvider(
            parameters: salesParameters,
            variables: salesVariables);
        var productionSolutionProvider = new ProductionSolutionProvider(productionParameters, productionVariables);
        var systemSolution = new SystemSolutionProvider(solutionProvider, productionSolutionProvider);
        return systemSolution;
    }
}
