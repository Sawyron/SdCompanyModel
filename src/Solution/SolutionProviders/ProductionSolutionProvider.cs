using Domain;
using Solution.Conditions;

namespace Solution.SolutionProviders;

public class ProductionSolutionProvider : SolutionProvider
{
    private readonly ProductionParameters _parameters;
    private readonly ProductionVariables _variables;

    public ProductionSolutionProvider(ProductionParameters parameters, ProductionVariables variables)
    {
        _parameters = parameters;
        _variables = variables;
    }

    public override IDictionary<string, double> GetFirstStep()
    {
        return new Dictionary<string, double>
        {
            {"y1", _variables.Y1 },
            {"y2", _variables.Y2 },
            {"y3", _variables.Y3 },
            {"y4", _variables.Y4 },
            {"y5", _variables.Y5 },
            {"v1", _variables.V1 },
            {"v2", _variables.V2 },
            {"v3", _variables.V3 },
            {"v4", _variables.V4 },
            {"v5", _variables.V5 },
            {"v6", _variables.V6 },
            {"v7", _variables.V7 },
            {"v8", _variables.V8 },
            {"v9", _variables.V9 },
            {"v10", _variables.V10 },
            {"v11", _variables.V11 },
            {"f3", _variables.F3 }
        };
    }

    public override IDictionary<string, double> ResolveStep(
        IDictionary<string, double> previousStep,
        IDictionary<string, double> externals,
        double interval)
    {
        double y3 = ProductionSystem.Y3(
                        previousStep["y3"],
                        interval,
                        _parameters.T4,
                        previousStep["w11"]);
        double y5 = ProductionSystem.Y5(
                        previousStep["y5"],
                        interval,
                        previousStep["v10"],
                        previousStep["v1"]);
        double y4 = ProductionSystem.Y4(
                        previousStep["y4"],
                        interval,
                        previousStep["v6"],
                        previousStep["v10"]);
        double v6 = ProductionSystem.V6(
                        previousStep["y1"],
                        previousStep["y2"],
                        _parameters.T5,
                        previousStep["v5"],
                        previousStep["v7"],
                        previousStep["v8"],
                        previousStep["v9"],
                        previousStep["w11"]);
        double v5 = ProductionSystem.V5(_parameters.K, y3);
        double v1 = ProductionSystem.V1(
                        interval,
                        previousStep["v10"],
                        _parameters.T7,
                        previousStep["v1"]);
        double y2 = ProductionSystem.Y2(
                        previousStep["y2"],
                        interval,
                        previousStep["v1"],
                        previousStep["f3"]);
        double y1 = ProductionSystem.Y1(
                        previousStep["y1"],
                        interval,
                        previousStep["w11"],
                        previousStep["f3"]);
        double v3 = ProductionSystem.V3(_parameters.T2, _parameters.T3, v5, y2);
        double v2 = ProductionSystem.V2(y1, v3);
        double v4 = ProductionSystem.V4(interval, y2);
        return new Dictionary<string, double>
        {
            {"v1", v1 },
            {"y5", y5 },
            {"v10", ProductionSystem.V10(
                interval,
                previousStep["v6"],
                _parameters.T6,
                previousStep["v10"]) },
            {"y4", y4 },
            {"y3", y3 },
            {"v9", ProductionSystem.V9(
                _parameters.T2,
                _parameters.T7,
                y3) },
            {"v8", ProductionSystem.V8(y4, y5) },
            {"v7", ProductionSystem.V7(
                _parameters.T6,
                _parameters.T7,
                y3) },
            {"v6", v6 },
            {"v11", ProductionSystem.V11(v6, _parameters.Beta) },
            {"v5", v5 },
            {"y2", y2 },
            {"y1", y1 },
            {"v3", v3 },
            {"v4", v4 },
            {"v2", v2 },
            {"f3", ProductionSystem.F3(v2, v4) }
        };
    }
}
