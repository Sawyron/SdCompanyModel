using Solution.Conditions;

namespace Solution.SolutionProviders;

public class SystemSolutionProvider
{
    private readonly SalesSolutionProvider _salesSolution;
    private readonly ProductionSolutionProvider _productionSolution;

    public SystemSolutionProvider(SalesSolutionProvider salesSolution, ProductionSolutionProvider productionSolution)
    {
        _salesSolution = salesSolution;
        _productionSolution = productionSolution;
    }

    public IEnumerable<SolutionStep> GetSolution(double interval, double end)
    {
        static void TransferDependencies(IDictionary<string, double> sales, IDictionary<string, double> production)
        {
            sales["y1"] = production["y1"];
            sales["v3"] = production["v3"];
            sales["f3"] = production["f3"];
            production["w11"] = sales["w11"];
        }
        var currentProductionStep = _productionSolution.GetFirstStep();
        var currentSalesStep = _salesSolution.GetFirstStep();
        TransferDependencies(currentSalesStep, currentProductionStep);
        yield return MapStepsToSolutionStep(currentSalesStep, currentProductionStep, 0);
        int steps = Convert.ToInt32(end / interval) + 1;
        for (int i = 1; i < steps; i++)
        {
            var nextProductionStep = _productionSolution.ResolveStep(currentProductionStep, interval);
            var nextSalesStep = _salesSolution.ResolveStep(currentSalesStep, interval);
            TransferDependencies(nextSalesStep, nextProductionStep);
            yield return MapStepsToSolutionStep(nextSalesStep, nextProductionStep, i * interval);
            currentSalesStep = nextSalesStep;
            currentProductionStep = nextProductionStep;
        }
    }

    private static SolutionStep MapStepsToSolutionStep(
        IDictionary<string, double> sales,
        IDictionary<string, double> production,
        double time) => new(
            time,
            sales.Concat(production)
                .GroupBy(pair => pair.Key)
                .ToDictionary(group => group.Key, group => group.First().Value));
}