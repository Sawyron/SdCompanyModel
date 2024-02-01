using Solution.Common;
using Solution.Production;
using Solution.Sales;

namespace Solution.Company;
public class CompanyStepResolver : IStepResolver
{
    private readonly ProductionStepResolver _productionResolver;
    private readonly SalesStepResolver _salesResolver;

    public CompanyStepResolver(ProductionStepResolver productionResolver, SalesStepResolver salesResolver)
    {
        _productionResolver = productionResolver;
        _salesResolver = salesResolver;
    }

    public IDictionary<string, double> ResolveStep(IReadOnlyDictionary<string, double> variables, double interval)
    {
        var productionStep = _productionResolver.ResolveStep(variables, interval);
        var variablesForSales = variables.ToDictionary();
        variablesForSales["y1_cur"] = productionStep["y1"];
        variablesForSales["v3_cur"] = productionStep["v3"];
        var salesStep = _salesResolver.ResolveStep(variablesForSales, interval);
        return productionStep.Concat(salesStep)
            .GroupBy(pair => pair.Key)
            .ToDictionary(g => g.Key, g => g.First().Value);
    }
}
