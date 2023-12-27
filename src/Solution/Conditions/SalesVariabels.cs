using Domain;

namespace Solution.Conditions;

public record SalesVariables(
    double X1,
    double X2,
    double X3,
    double F1,
    double X4,
    double X5,
    double W5,
    double X6,
    double W8,
    double W9,
    double W3,
    double W11,
    double W6,
    double W7,
    double W10,
    double W1)
{
    public static SalesVariables CreateFromInitial(
        double x3,
        double w1,
        double y1,
        double v3,
        SalesParameters parameters)
    {
        double x2 = SalesSystem.W5(parameters.K, x3);
        double x4 = parameters.OrderProcessDelay * parameters.Demands;
        double x5 = parameters.LinkDelay * parameters.Demands;
        double x6 = parameters.ShipmentDelay * parameters.Demands;
        double x1 = SalesSystem.W9(
                        x3,
                        parameters.OrderFulfillmentDelay,
                        parameters.AbsenceDelay);
        return new(
            X1: x1,
            X2: x2,
            X3: x3,
            X4: x4,
            X5: x5,
            X6: x6,
            F1: x3,
            W3: SalesSystem.W3(
                parameters.OrderFulfillmentDelay,
                parameters.AbsenceDelay,
                x2,
                x2),
            W11: parameters.Demands,
            W6: x3,
            W10: x1,
            W1: w1,
            W5: SalesSystem.W5(parameters.K, x3),
            W8: SalesSystem.W8(
                x4,
                x5,
                x6,
                y1),
            W7: SalesSystem.W7(
                x3,
                v3,
                parameters.OrderProcessDelay,
                parameters.LinkDelay,
                parameters.ShipmentDelay),
            W9: SalesSystem.W9(
                x3,
                parameters.OrderFulfillmentDelay,
                parameters.AbsenceDelay));
    }
}
