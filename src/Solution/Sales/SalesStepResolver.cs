using Domain;
using Solution.Common;
using Solution.Sales.Conditions;

namespace Solution.Sales;
public class SalesStepResolver : IStepResolver
{
    private readonly SalesParameters _parameters;
    private readonly ThirdOrderDelay _w1Delay;
    private readonly ThirdOrderDelay _w10Delay;

    public SalesStepResolver(SalesParameters parameters)
    {
        _parameters = parameters;
        _w1Delay = new(parameters.T8);
        _w10Delay = new(parameters.T6);
    }

    public IDictionary<string, double> ResolveStep(IReadOnlyDictionary<string, double> variables, double interval)
    {
        double x3 = SalesSystem.X3(
                        interval,
                        variables["x3"],
                        _parameters.Uk,
                        _parameters.T4);
        double w5 = SalesSystem.W5(_parameters.K, x3);
        double x2 = SalesSystem.X2(
                        variables["x2"],
                        interval,
                        variables["w1"],
                        variables["f1"]);
        double x1 = SalesSystem.X1(
                        variables["x1"],
                        interval,
                        _parameters.Uk,
                        variables["f1"]);
        double w3 = SalesSystem.W3(
                        _parameters.T2,
                        _parameters.T3,
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
        double w1 = _w1Delay.GetDelayValues(interval, variables["f3"]).Rate;
        double w10 = _w10Delay.GetDelayValues(interval, variables["w6"]).Rate;
        return new Dictionary<string, double>()
        {
            {"w1", w1 },
            {"w3", w3 },
            {"w5", w5 },
            {"w6", SalesSystem.W6(
                variables["x1"],
                variables["x2"],
                _parameters.Uk,
                variables["w5"],
                _parameters.T5,
                variables["w7"],
                variables["w8"],
                variables["w9"]) },
            {"w7", SalesSystem.W7(
                x3,
                variables["v3_cur"],
                _parameters.T6,
                _parameters.T7,
                _parameters.T8) },
            {"w8", SalesSystem.W8(x4, x5, x6, variables["y1_cur"]) },
            {"w9", SalesSystem.W9(
                x3,
                _parameters.T2,
                _parameters.T3) },
            {"w10", w10 },
            {"w11", SalesSystem.W11(
                interval,
                variables["w10"],
                _parameters.T7,
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
