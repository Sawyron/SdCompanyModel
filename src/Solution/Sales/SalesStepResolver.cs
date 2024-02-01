using Domain;
using Solution.Common;
using Solution.Sales.Conditions;

namespace Solution.Sales;
public class SalesStepResolver : IStepResolver
{
    private readonly Func<SalesParameters> _parametersFactory;

    public SalesStepResolver(Func<SalesParameters> parametersFactory)
    {
        _parametersFactory = parametersFactory;
    }

    public IDictionary<string, double> ResolveStep(IReadOnlyDictionary<string, double> variables, double interval)
    {
        var parameters = _parametersFactory();
        double x3 = SalesSystem.X3(
                        interval,
                        variables["x3"],
                        parameters.Uk,
                        parameters.T4);
        double w5 = SalesSystem.W5(parameters.K, x3);
        double x2 = SalesSystem.X2(
                        variables["x2"],
                        interval,
                        variables["w1"],
                        variables["f1"]);
        double x1 = SalesSystem.X1(
                        variables["x1"],
                        interval,
                        parameters.Uk,
                        variables["f1"]);
        double w3 = SalesSystem.W3(
                        parameters.T2,
                        parameters.T3,
                        x2,
                        w5);
        double x4 = SalesSystem.X4(
                        variables["x4"],
                        interval,
                        variables["x6"],
                        variables["w10"]);
        double x5 = SalesSystem.X5(
                        variables["x5"],
                        interval,
                        variables["w10"],
                        variables["w11"]);
        double x6 = SalesSystem.X6(
                        variables["x6"],
                        interval,
                        variables["f3"],
                        variables["w1"]);
        return new Dictionary<string, double>()
        {
            {"w1", SalesSystem.W1(
                interval,
                variables["f3"],
                parameters.T8,
                variables["w1"]) },
            {"w3", w3 },
            {"w5", w5 },
            {"w6", SalesSystem.W6(
                variables["x1"],
                variables["x2"],
                parameters.Uk,
                variables["w5"],
                parameters.T5,
                variables["w7"],
                variables["w8"],
                variables["w9"]) },
            {"w7", SalesSystem.W7(
                x3,
                variables["v3_cur"],
                parameters.T6,
                parameters.T7,
                parameters.T8) },
            {"w8", SalesSystem.W8(x4, x5, x6, variables["y1_cur"]) },
            {"w9", SalesSystem.W9(
                x3,
                parameters.T2,
                parameters.T3) },
            {"w10", SalesSystem.W10(
                interval,
                variables["w6"],
                parameters.T6,
                variables["w10"]) },
            {"w11", SalesSystem.W11(
                interval,
                variables["w10"],
                parameters.T7,
                variables["w11"]) },
            {"x1", x1 },
            {"x2", x2 },
            {"x3", x3 },
            {"x4", x4 },
            {"x5", x5 },
            {"x6", x6 },
            {"f1", SalesSystem.F1(
                SalesSystem.W2(x1, w3),
                SalesSystem.W4(x2, interval)) }
        };
    }
}
