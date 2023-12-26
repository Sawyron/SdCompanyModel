using ScottPlot;
using Solution.Conditions;
using Solution.Solution;
using System.Windows;

namespace WpfUi;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        PlotView.Plot.XLabel("time, weeks");
        try
        {
            var result = (await GetResults()).ToList();
            var time = result.Select(tuple => tuple.Time).ToArray();
            var solutions = result.Select(tuple => tuple.Values)
                .SelectMany(dict => dict)
                .Where(pair => pair.Key.StartsWith('y'))
                .GroupBy(pair => pair.Key)
                .ToDictionary(g => g.Key, g => g.Select(pair => pair.Value).ToArray());
            foreach ((string label, double[] values) in solutions)
            {
                PlotView.Plot.AddScatter(time, values, label: label, markerShape: MarkerShape.none);
            }
            PlotView.Plot.Legend();
            PlotView.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    private async Task<IEnumerable<SolutionStep>> GetResults()
    {
        double ui = 1000;
        double productionT2 = 1;
        double productionT3 = 1;
        double interval = 0.05;
        double y1 = (productionT2 + productionT3) * ui;
        var productionParameters = new ProductionParameters(
            W11: ui,
            Y1: y1,
            T2: productionT2,
            T3: productionT3,
            T4: 8,
            T5: 4,
            T6: 1,
            T7: 6,
            K: 4,
            Beta: 1000 * ui
            );
        var productionVariables = ProductionVariables.CreateFromInitial(
            w11:ui,
            y3:ui,
            v1:1000,
            v6: 1000,
            v10: 1000,
            v11: 1000,
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
        return await Task.Run(() => systemSolution.GetSolution(interval, 50));
    }
}