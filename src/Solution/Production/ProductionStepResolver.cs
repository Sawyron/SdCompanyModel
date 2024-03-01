using Domain;
using Solution.Common;
using Solution.Production.Conditions;

namespace Solution.Production;
public class ProductionStepResolver : IStepResolver
{
    private readonly ProductionParameters _parameters;
    private readonly ThirdOrderDelay _v1Delay;
    private readonly ThirdOrderDelay _v10Delay;

    public ProductionStepResolver(ProductionParameters parameters)
    {
        _parameters = parameters;
        _v1Delay = new(parameters.T7);
        _v10Delay = new(parameters.T6);
    }

    public IDictionary<string, double> ResolveStep(IReadOnlyDictionary<string, double> variables, double interval)
    {
        double y3 = ProductionSystem.Y3(
                        variables["y3"],
                        interval,
                        _parameters.T4,
                        variables["w11"]);
        double y5 = ProductionSystem.Y5(
                        variables["y5"],
                        interval,
                        variables["v10"],
                        variables["v1"]);
        double y4 = ProductionSystem.Y4(
                        variables["y4"],
                        interval,
                        variables["v6"],
                        variables["v10"]);
        double v6 = ProductionSystem.V6(
                        variables["y1"],
                        variables["y2"],
                        _parameters.T5,
                        variables["v5"],
                        variables["v7"],
                        variables["v8"],
                        variables["v9"],
                        variables["w11"]);
        double v5 = ProductionSystem.V5(_parameters.K, y3);
        double v1 = _v1Delay.GetDelayValues(interval, variables["v10"]).Rate;
        double y2 = ProductionSystem.Y2(
                        variables["y2"],
                        interval,
                        variables["v1"],
                        variables["f3"]);
        double y1 = ProductionSystem.Y1(
                        variables["y1"],
                        interval,
                        variables["w11"],
                        variables["f3"]);
        double v3 = ProductionSystem.V3(_parameters.T2, _parameters.T3, v5, y2);
        double v2 = ProductionSystem.V2(y1, v3);
        double v4 = ProductionSystem.V4(interval, y2);
        double v10 = _v10Delay.GetDelayValues(interval, variables["v6"]).Rate;
        return new Dictionary<string, double>
        {
            {"v1", v1 },
            {"v2", v2 },
            {"v3", v3 },
            {"v4", v4 },
            {"v5", v5 },
            {"v6", v6 },
            {"v7", ProductionSystem.V7(
                _parameters.T6,
                _parameters.T7,
                y3) },
            {"v8", ProductionSystem.V8(y4, y5) },
            {"v9", ProductionSystem.V9(
                _parameters.T2,
                _parameters.T7,
                y3) },
            {"v10", v10 },
            {"v11", ProductionSystem.V11(v6, _parameters.Beta) },
            {"y1", y1 },
            {"y2", y2 },
            {"y3", y3 },
            {"y4", y4 },
            {"y5", y5 },
            {"f3", ProductionSystem.F3(v2, v4) }
        };
    }
}
