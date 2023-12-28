using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfUi.SystemSettings.Production;

/// <summary>
/// Interaction logic for ProductionSettingsView.xaml
/// </summary>
public partial class ProductionSettingsView : UserControl
{
    public ProductionSettingsView()
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            DataContext = ((App)Application.Current).Services.GetRequiredService<ProductionSettingsViewModel>();
        }
    }
}
