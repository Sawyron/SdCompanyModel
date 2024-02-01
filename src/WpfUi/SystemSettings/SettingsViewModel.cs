using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solution.Common;
using System.Collections.ObjectModel;

namespace WpfUi.SystemSettings;

public class SettingsViewModel : ObservableObject
{
    private readonly ParameterInputService _inputService;

    public SettingsViewModel(ParameterInputService inputService)
    {
        _inputService = inputService;
        Groups = new()
        {
            new ParameterGroup
            {
                Name = "Main",
                Parameters = new(_inputService.GetParameters()
                    .Select(p => new ParameterInput {Name = p.Key, Value = p.Value}))
            }
        };
        SaveParametersCommand = new RelayCommand(() =>
            _inputService.UpdateParameters(Groups.First().Parameters.ToDictionary(p => p.Name, p => p.Value)));
    }

    public ObservableCollection<ParameterGroup> Groups { get; set; }
    public IRelayCommand SaveParametersCommand { get; }
}
