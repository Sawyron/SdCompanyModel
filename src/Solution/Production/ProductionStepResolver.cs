using Domain;
using Solution.Common;
using Solution.Production.Conditions;

namespace Solution.Production;
public class ProductionStepResolver : IStepResolver
{
    private readonly Func<ProductionParameters> _parametersFactory;

    public ProductionStepResolver(Func<ProductionParameters> parametersFactory)
    {
        _parametersFactory = parametersFactory;
    }

    public IDictionary<string, double> ResolveStep(IReadOnlyDictionary<string, double> variables, double interval)
    {
        var parameters = _parametersFactory();
        double y3 = ProductionSystem.Y3(
                        variables["y3"],
                        interval,
                        parameters.T4,
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
                        parameters.T5,
                        variables["v5"],
                        variables["v7"],
                        variables["v8"],
                        variables["v9"],
                        variables["w11"]);
        double v5 = ProductionSystem.V5(parameters.K, y3);
        double v1 = ProductionSystem.V1(
                        interval,
                        variables["v10"],
                        parameters.T7,
                        variables["v1"]);
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
        double v3 = ProductionSystem.V3(parameters.T2, parameters.T3, v5, y2);
        double v2 = ProductionSystem.V2(y1, v3);
        double v4 = ProductionSystem.V4(interval, y2);
        return new Dictionary<string, double>
        {
            {"v1", v1 },
            {"v2", v2 },
            {"v3", v3 },
            {"v4", v4 },
            {"v5", v5 },
            {"v6", v6 },
            {"v7", ProductionSystem.V7(
                parameters.T6,
                parameters.T7,
                y3) },
            {"v8", ProductionSystem.V8(y4, y5) },
            {"v9", ProductionSystem.V9(
                parameters.T2,
                parameters.T7,
                y3) },
            {"v10", ProductionSystem.V10(
                interval,
                variables["v6"],
                parameters.T6,
                variables["v10"]) },
            {"v11", ProductionSystem.V11(v6, parameters.Beta) },
            {"y1", y1 },
            {"y2", y2 },
            {"y3", y3 },
            {"y4", y4 },
            {"y5", y5 },
            {"f3", ProductionSystem.F3(v2, v4) }
        };
    }
}
