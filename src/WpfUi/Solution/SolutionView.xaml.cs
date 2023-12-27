using CommunityToolkit.Mvvm.Messaging;
using ScottPlot;
using System.Windows.Controls;
using WpfUi.Solution.Messages;

namespace WpfUi.Solution;

/// <summary>
/// Interaction logic for SolutionView.xaml
/// </summary>
public partial class SolutionView : UserControl
{
    public SolutionView()
    {
        InitializeComponent();
        DataContext = new SolutionViewModel();
        WeakReferenceMessenger.Default.Register<SolutionView, DrawSolutionMessage>(this, (r, m) =>
        {
            PlotView.Plot.Clear();
            var result = m.Value.Steps;
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
        });
    }
}
