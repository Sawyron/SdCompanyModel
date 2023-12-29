using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Infrastructure.Solution;
using Microsoft.Win32;
using Solution.SolutionProviders;
using System.IO;
using System.Windows;
using WpfUi.Solution.Messages;

namespace WpfUi.Solution;

public class SolutionViewModel : ObservableObject
{
    private static readonly Func<string, bool> _productionFilter =
        (name) => name.StartsWith('y');
    private static readonly Func<string, bool> _salesFilter =
        (name) => name.StartsWith('x');

    private readonly SolutionService _solutionService;
    private readonly Func<double, SystemSolutionProvider> _solutionFactory;
    private SystemSolutionResult? _solutionResult;
    private Func<string, bool> _solutionFilter = _productionFilter;

    public SolutionViewModel(SolutionService solutionService, Func<double, SystemSolutionProvider> solutionFactory)
    {
        _solutionService = solutionService;
        _solutionFactory = solutionFactory;
        ExportSolutionCommand = new AsyncRelayCommand(ExportSolution, () => _solutionResult is not null);
        GetSolutionCommand = new AsyncRelayCommand(GetSolution);
        SelectFilterCommand = new RelayCommand<Func<string, bool>>(
            (filter) =>
            {
                _solutionFilter = filter is not null ? filter : _solutionFilter;
                if (_solutionResult is not null)
                {
                    WeakReferenceMessenger.Default.Send(MapResultToDrawMessage(_solutionResult, _solutionFilter));
                }
            });
    }

    public IAsyncRelayCommand GetSolutionCommand { get; }
    public IAsyncRelayCommand ExportSolutionCommand { get; }
    public IRelayCommand<Func<string, bool>> SelectFilterCommand { get; }
    public Func<string, bool> ProductionFilter => _productionFilter;
    public Func<string, bool> SalesFilter => _salesFilter;

    private async Task GetSolution()
    {
        double interval = 0.05;
        var result = await Task.Run(() => _solutionService.GetSolution(interval, 50, _solutionFactory(interval)));
        _solutionResult = result;
        ExportSolutionCommand.NotifyCanExecuteChanged();
        var message = MapResultToDrawMessage(result, _solutionFilter);
        WeakReferenceMessenger.Default.Send(message);
    }

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

    private static DrawSolutionMessage MapResultToDrawMessage(SystemSolutionResult result, Func<string, bool> filter)
    {
        var time = result.Steps.Select(tuple => tuple.Time).ToArray();
        var solutions = result.Steps.Select(tuple => tuple.Values)
            .SelectMany(dict => dict)
            .GroupBy(pair => pair.Key)
            .Where(pair => filter(pair.Key))
            .ToDictionary(g => g.Key, g => g.Select(pair => pair.Value).ToArray());
        return new DrawSolutionMessage(time, solutions);
    }
}