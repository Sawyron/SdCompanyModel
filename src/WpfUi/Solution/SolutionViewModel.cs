using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Infrastructure.Solution;
using Microsoft.Win32;
using Solution.Conditions;
using Solution.SolutionProviders;
using System.IO;
using System.Windows;
using WpfUi.Solution.Messages;

namespace WpfUi.Solution;

public class SolutionViewModel : ObservableObject
{
    private SystemSolutionResult? _solutionResult;

    public SolutionViewModel()
    {
        ExportSolutionCommand = new AsyncRelayCommand(ExportSolution, () => _solutionResult is not null);
        GetSolutionCommand = new AsyncRelayCommand(async () =>
        {
            var result = await GetResults();
            _solutionResult = result;
            ExportSolutionCommand.NotifyCanExecuteChanged();
            WeakReferenceMessenger.Default.Send(new DrawSolutionMessage(result));
        });
    }

    public IAsyncRelayCommand GetSolutionCommand { get; }
    public IAsyncRelayCommand ExportSolutionCommand { get; }

    private async Task ExportSolution()
    {
        if (_solutionResult is null)
        {
            MessageBox.Show("No solution", "Ops...", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var dialog = new SaveFileDialog
        {
            Filter = "CSV (*.csv)|*.csv"
        };
        if (dialog.ShowDialog() == true)
        {
            var solutionService = new SolutionService();
            string fileName = dialog.FileName;
            using var fs = new FileStream(fileName, FileMode.Create);
            await solutionService.WriteToStreamAsCsvAsync(_solutionResult, fs);
        }
    }

    private async Task<SystemSolutionResult> GetResults()
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
            w11: ui,
            y3: ui,
            v1: 1000,
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
        var solutionService = new SolutionService();
        return await Task.Run(() => solutionService.GetSolution(interval, 50, systemSolution));
    }
}