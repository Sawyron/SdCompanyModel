using Solution.Common;
using Solution.Production.Conditions;

namespace Solution.Production;
public class ProductionParametersProvider
{
    private readonly ParameterInputService _inputService;

    public ProductionParametersProvider(ParameterInputService inputService)
    {
        _inputService = inputService;
    }

    public ProductionParameters GetProductionParameters()
    {
        IReadOnlyDictionary<string, double> input = _inputService.GetParameters();
        return new ProductionParameters(
            input["t2"],
            input["t3"],
            input["t4"],
            input["t5"],
            input["t6"],
            input["t7"],
            input["K2"],
            input["Beta"]);
    }
}
