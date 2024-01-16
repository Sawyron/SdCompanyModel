using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;

namespace WpfUi.SystemSettings;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            DataContext = ((App)(Application.Current)).Services.GetRequiredService<SettingsViewModel>();
        }
    }
}
