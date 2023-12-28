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
    private readonly SolutionService _solutionService;
    private readonly Func<double, SystemSolutionProvider> _solutionFactory;
    private SystemSolutionResult? _solutionResult;

    public SolutionViewModel(SolutionService solutionService, Func<double, SystemSolutionProvider> solutionFactory)
    {
        _solutionService = solutionService;
        _solutionFactory = solutionFactory;
        ExportSolutionCommand = new AsyncRelayCommand(ExportSolution, () => _solutionResult is not null);
        GetSolutionCommand = new AsyncRelayCommand(async () =>
        {
            double interval = 0.05;
            var result = await Task.Run(() => _solutionService.GetSolution(interval, 50, _solutionFactory(interval)));
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
}