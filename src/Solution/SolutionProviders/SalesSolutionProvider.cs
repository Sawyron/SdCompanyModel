using Domain;
using Solution.Conditions;

namespace Solution.SolutionProviders;

public class SalesSolutionProvider
{
    private readonly SalesParameters _parameters;
    private readonly SalesVariables _variables;

    public SalesSolutionProvider(SalesParameters parameters, SalesVariables variables)
    {
        _parameters = parameters;
        _variables = variables;
    }

    public IEnumerable<SolutionStep> GetSolution(double interval, double end)
    {
        int steps = Convert.ToInt32(end / interval) + 1;
        var currentStep = GetFirstStep();
        yield return new SolutionStep(0, currentStep);
        for (int i = 1; i < steps; i++)
        {
            var nextStep = ResolveStep(currentStep, interval);
            yield return new SolutionStep(i * interval, nextStep);
            currentStep = nextStep;
        }
    }

    public IDictionary<string, double> GetFirstStep() => new Dictionary<string, double>
        {
            {"x1", _variables.X1  },
            {"x2", _variables.X2 },
            {"x3", _variables.X3 },
            {"x4", _variables.X4 },
            {"x5", _variables.X5 },
            {"x6", _variables.X6 },
            {"f1", _variables.F1 },
            {"w11", _variables.W11 },
            {"w6", _variables.W6 },
            {"w10", _variables.W10 },
            {"w1", _variables.W1 },
            {"w5", _variables.W5 },
            {"w7", _variables.W7},
            {"w8", _variables.W8 },
            {"w9", _variables.W9 },
            {"w3", _variables.W3 }
        };

    public IDictionary<string, double> ResolveStep(IDictionary<string, double> previousStep, double interval)
    {
        double x3 = SalesSystem.X3(
                        interval,
                        previousStep["x3"],
                        _parameters.Demands,
                        _parameters.AveragingDelay);
        double w5 = SalesSystem.W5(_parameters.K, x3);
        double x2 = SalesSystem.X2(
                        previousStep["x2"],
                        interval,
                        previousStep["w1"],
                        previousStep["f1"]);
        double x1 = SalesSystem.X1(
                        previousStep["x1"],
                        interval,
                        _parameters.Demands,
                        previousStep["f1"]);
        double w3 = SalesSystem.W3(
                        _parameters.OrderFulfillmentDelay,
                        _parameters.AbsenceDelay,
                        x2,
                        w5);
        double x4 = SalesSystem.X4(
                        previousStep["x4"],
                        interval,
                        previousStep["x6"],
                        previousStep["w10"]);
        double x5 = SalesSystem.X5(
                        previousStep["x5"],
                        interval,
                        previousStep["w10"],
                        previousStep["w11"]);
        double x6 = SalesSystem.X6(
                        previousStep["x6"],
                        interval,
                        previousStep["f3"],
                        previousStep["w1"]);
        return new Dictionary<string, double>()
        {
            { "w1", SalesSystem.W1(
                interval,
                previousStep["f3"],
                _parameters.ShipmentDelay,
                previousStep["w1"]) },
            { "x6", x6 },
            {"w11", SalesSystem.W11(
                interval,
                previousStep["w10"],
                _parameters.ShipmentDelay,
                previousStep["w11"]) },
            {"x5", x5 },
            {"w10", SalesSystem.W10(
                interval,
                previousStep["w6"],
                _parameters.OrderProcessDelay,
                previousStep["w10"]) },
            {"x4", x4 },
            {"w9", SalesSystem.W9(
                x3,
                _parameters.OrderProcessDelay,
                _parameters.AbsenceDelay) },
            {"w8", SalesSystem.W8(x4, x5, x6, previousStep["y1"]) },
            {"w7", SalesSystem.W7(
                x3,
                previousStep["v3"],
                _parameters.OrderProcessDelay,
                _parameters.LinkDelay,
                _parameters.ShipmentDelay) },
            {"w6", SalesSystem.W6(
                previousStep["x1"],
                previousStep["x2"],
                _parameters.Demands,
                previousStep["w5"],
                _parameters.StockControlDelay,
                previousStep["w7"],
                previousStep["w8"],
                previousStep["w9"]) },
            {"x3", x3 },
            {"w5", w5 },
            {"w3", w3 },
            {"x2", x2 },
            {"x1", x1 },
            {"f1", SalesSystem.F1(
                SalesSystem.W2(x1, w3),
                SalesSystem.W4(x2, interval)) }
        };
    }
}
