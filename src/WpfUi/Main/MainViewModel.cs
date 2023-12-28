using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using WpfUi.SystemSettings;

namespace WpfUi.Main;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        ExitCommand = new RelayCommand(Exit);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
    }

    public IRelayCommand ExitCommand { get; }
    public IRelayCommand OpenSettingsCommand { get; }

    private void OpenSettings()
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.Show();
    }

    private void Exit()
    {
        Application.Current.Shutdown();
    }
}
