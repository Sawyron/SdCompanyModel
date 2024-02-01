using Solution.Common;
using Solution.Sales.Conditions;

namespace Solution.Sales;
public class SalesParametersProvider
{
    private readonly ParameterInputService _inputService;

    public SalesParametersProvider(ParameterInputService inputService)
    {
        _inputService = inputService;
    }

    public SalesParameters GetSalesParameters()
    {
        var input = _inputService.GetParameters();
        return new SalesParameters(
            input["ui"],
            input["K1"],
            input["T2"],
            input["T3"],
            input["T4"],
            input["T5"],
            input["T6"],
            input["T7"],
            input["T8"]);
    }
}
