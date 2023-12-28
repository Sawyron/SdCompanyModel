using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using WpfUi.Main;

namespace WpfUi;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            DataContext = ((App)Application.Current).Services.GetRequiredService<MainViewModel>();
        }
    }
}