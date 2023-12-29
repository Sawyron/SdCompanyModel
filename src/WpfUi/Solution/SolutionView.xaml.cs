using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using ScottPlot;
using System.ComponentModel;
using System.Windows;
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
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            DataContext = ((App)Application.Current).Services.GetRequiredService<SolutionViewModel>();
        }
        WeakReferenceMessenger.Default.Register<SolutionView, DrawSolutionMessage>(this, (r, m) =>
        {
            PlotView.Plot.Clear();
            var (time, solutions) = m.Value;
            foreach ((string label, double[] values) in solutions)
            {
                PlotView.Plot.AddScatter(time, values, label: label, markerShape: MarkerShape.none);
            }
            PlotView.Plot.Legend();
            PlotView.Refresh();
        });
    }
}
