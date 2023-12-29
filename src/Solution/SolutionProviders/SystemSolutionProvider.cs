namespace Solution.SolutionProviders;

public class SystemSolutionProvider : SolutionProvider
{
    private readonly SalesSolutionProvider _salesSolution;
    private readonly ProductionSolutionProvider _productionSolution;

    public SystemSolutionProvider(SalesSolutionProvider salesSolution, ProductionSolutionProvider productionSolution)
    {
        _salesSolution = salesSolution;
        _productionSolution = productionSolution;
    }
    public override IDictionary<string, double> GetFirstStep()
    {
        var productionStep = _productionSolution.GetFirstStep();
        var salesStep = _salesSolution.GetFirstStep();
        TransferDependencies(salesStep, productionStep);
        return CombineValues(salesStep, productionStep);
    }

    public override IDictionary<string, double> ResolveStep(
        IDictionary<string, double> previousStep,
        IDictionary<string, double> externals,
        double interval)
    {
        var productionStep = _productionSolution.ResolveStep(previousStep, new Dictionary<string, double>(), interval);
        var salesStep = _salesSolution.ResolveStep(
            previousStep,
            new Dictionary<string, double>
            {
                {"f3", productionStep["f3"] },
                {"y1", productionStep["y1"] },
                {"v3", productionStep["v3"] }
            },
            interval);
        TransferDependencies(salesStep, productionStep);
        return CombineValues(salesStep, productionStep);
    }

    private static void TransferDependencies(IDictionary<string, double> sales, IDictionary<string, double> production)
    {
        sales["y1"] = production["y1"];
        sales["v3"] = production["v3"];
        sales["f3"] = production["f3"];
        production["w11"] = sales["w11"];
    }

    private static IDictionary<string, double> CombineValues(
        IDictionary<string, double> sales,
        IDictionary<string, double> production) =>
            sales.Concat(production)
                .GroupBy(pair => pair.Key)
                .ToDictionary(group => group.Key, group => group.First().Value);
}