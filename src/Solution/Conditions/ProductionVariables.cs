using Domain;

namespace Solution.Conditions;

public record ProductionVariables(
    double Y1,
    double Y2,
    double Y3,
    double Y4,
    double Y5,
    double V1,
    double V2,
    double V3,
    double V4,
    double V5,
    double V6,
    double V7,
    double V8,
    double V9,
    double V10,
    double V11,
    double F3)
{
    public static ProductionVariables CreateFromInitial(
        double w11,
        double y3,
        double v1,
        double v6,
        double v10,
        double v11,
        double interval,
        ProductionParameters parameters)
    {
        double y1 = (parameters.T2 + parameters.T3) * y3;
        double y2 = parameters.K * y3;
        double y4 = parameters.T6 * w11;
        double y5 = parameters.T7 * w11;
        double v5 = ProductionSystem.V5(parameters.K, y3);
        double v4 = ProductionSystem.V4(interval, y2);
        double v3 = ProductionSystem.V3(parameters.T2, parameters.T3, v5, y2);
        double v2 = ProductionSystem.V2(y1, v3);
        return new ProductionVariables(
            Y1: y1,
            Y2: y2,
            Y3: y3,
            Y4: y4,
            Y5: y5,
            V1: v1,
            V2: v2,
            V3: v3,
            V4: v4,
            V5: v5,
            V6: v6,
            V7: ProductionSystem.V7(parameters.T6, parameters.T7, y3),
            V8: ProductionSystem.V8(y4, y5),
            V9: ProductionSystem.V9(parameters.T2, parameters.T7, y3),
            V10: v10,
            V11: v11,
            F3: ProductionSystem.F3(v2, v4));
    }
}
