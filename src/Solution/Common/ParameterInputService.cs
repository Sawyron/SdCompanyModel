namespace Solution.Common;

public class ParameterInputService
{
    private readonly Dictionary<string, double> _currentParameters;

    public ParameterInputService() : this(new Dictionary<string, double>())
    { }
    public ParameterInputService(IDictionary<string, double> parameters)
    {
        _currentParameters = new Dictionary<string, double>(parameters);
    }
    public IReadOnlyDictionary<string, double> GetParameters() => _currentParameters;

    public void UpdateParameters(IDictionary<string, double> parameters)
    {
        _currentParameters.Clear();
        foreach ((string name, double value) in parameters)
        {
            _currentParameters[name] = value;
        }
    }
}
