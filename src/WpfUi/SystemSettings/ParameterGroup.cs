using System.Collections.ObjectModel;

namespace WpfUi.SystemSettings;
public class ParameterGroup
{
    public required string Name { get; set; }
    public required ObservableCollection<ParameterInput> Parameters { get; set; }
}
