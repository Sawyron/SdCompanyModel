using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Infrastructure.Solution;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using WpfUi.Solution.Messages;

namespace WpfUi.Solution;

public class SolutionViewModel : ObservableObject
{
    private static readonly Func<string, bool> _productionFilter =
        (name) =>
            name == "uk" ||
            name == "w11" ||
            name == "y1" ||
            name == "y2" ||
            name == "y5" ||
            name == "v11";
    private static readonly Func<string, bool> _salesFilter =
        (name) =>
            name == "x1" ||
            name == "uk" ||
            name == "x1" ||
            name == "x2" ||
            name == "x6" ||
            name == "f1" ||
            name == "v3";
    private static readonly Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>> _absoluteMapper =
        group => group.Select(pair => pair.Value);
    private static readonly Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>> _percentMapper =
        group => group.Select(pair => pair.Value / group.First().Value * 100);

    private readonly SolutionService _solutionService;
    private SystemSolutionResult? _solutionResult;
    private Func<string, bool> _solutionFilter = _productionFilter;
    private Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>> _groupMapper = _absoluteMapper;

    public SolutionViewModel(SolutionService solutionService)
    {
        _solutionService = solutionService;
        ExportSolutionCommand = new AsyncRelayCommand(ExportSolution, () => _solutionResult is not null);
        GetSolutionCommand = new AsyncRelayCommand(GetSolution);
        SelectFilterCommand = new RelayCommand<Func<string, bool>>(
            filter =>
            {
                _solutionFilter = filter is not null ? filter : _solutionFilter;
                if (_solutionResult is not null)
                {
                    WeakReferenceMessenger.Default.Send(MapResultToDrawMessage(_solutionResult));
                }
            });
        SelectModeCommand = new RelayCommand<Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>>>(
            mapper =>
        {
            _groupMapper = mapper is not null ? mapper : _groupMapper;
            if (_solutionResult is not null)
            {
                WeakReferenceMessenger.Default.Send(MapResultToDrawMessage(_solutionResult));
            }
        });
    }

    public IAsyncRelayCommand GetSolutionCommand { get; }
    public IAsyncRelayCommand ExportSolutionCommand { get; }
    public IRelayCommand<Func<string, bool>> SelectFilterCommand { get; }
    public IRelayCommand<Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>>> SelectModeCommand { get; }
    public Func<string, bool> ProductionFilter => _productionFilter;
    public Func<string, bool> SalesFilter => _salesFilter;
    public Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>> AbsoluteMapper => _absoluteMapper;
    public Func<IGrouping<string, KeyValuePair<string, double>>, IEnumerable<double>> PercentMapper => _percentMapper;

    private async Task GetSolution()
    {
        double interval = 0.05;
        var result = await Task.Run(() => _solutionService.GetSolution(interval, 50));
        _solutionResult = result;
        ExportSolutionCommand.NotifyCanExecuteChanged();
        var message = MapResultToDrawMessage(result);
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
            using var fs = new FileStream(dialog.FileName, FileMode.Create);
            await _solutionService.WriteToStreamAsCsvAsync(_solutionResult, fs);
        }
    }

    private DrawSolutionMessage MapResultToDrawMessage(SystemSolutionResult result)
    {
        var time = result.Steps.Select(tuple => tuple.Time).ToArray();
        var solutions = result.Steps.Select(tuple => tuple.Values)
            .SelectMany(dict => dict)
            .GroupBy(pair => pair.Key)
            .Where(pair => _solutionFilter(pair.Key))
            .ToDictionary(g => g.Key, g => _groupMapper(g).ToArray());
        return new DrawSolutionMessage(time, solutions);
    }
}