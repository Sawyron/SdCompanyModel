using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfUi.SystemSettings;

public class SettingsViewModel : ObservableObject
{
    private readonly ParameterInputService _inputService;

    public SettingsViewModel(ParameterInputService inputService)
    {
        _inputService = inputService;
        Parameters = new(inputService.GetParameters()
            .Select(pair => new ParameterInput { Name = pair.Key, Value = pair.Value }));
    }

    public ObservableCollection<ParameterInput> Parameters { get; set; }
}
