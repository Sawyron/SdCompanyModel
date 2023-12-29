namespace Domain;

public static class SalesSystem
{
    public static double X1(
        double previous,
        double interval,
        double uk,
        double f1) => previous + interval * (uk - f1);

    public static double X2(
        double previous,
        double interval,
        double w2,
        double f1) => previous + interval * (w2 - f1);

    public static double W2(double x1, double w3) => x1 / w3;

    public static double W4(double x2, double interval) => x2 / interval;

    public static double F1(double w2, double w4) => Math.Min(w2, w4);

    public static double W3(
        double T2,
        double T3,
        double x2,
        double w5) => T2 + T3 * (w5 / x2);

    public static double W5(double k, double x3) => k * x3;

    public static double X3(
        double interval,
        double previous,
        double uk,
        double T4) => (1 - interval / T4) * previous + (interval / T4) * uk;

    public static double W6(
        double x1,
        double x2,
        double uk,
        double w5,
        double T5,
        double w7,
        double w8,
        double w9) => uk + (1 / T5) * (w5 - x2 + w7 - w8 + x1 - w9);

    public static double W7(
        double x3,
        double v3,
        double T6,
        double T7,
        double T8) => (v3 + T6 + T7 + T8) * x3;

    public static double W8(
        double x4,
        double x5,
        double x6,
        double y1) => x4 + x5 + x6 + y1;

    public static double W9(
        double x3,
        double T2,
        double T3) => (T2 + T3) * x3;

    public static double X4(
        double previous,
        double interval,
        double w6,
        double w10) => previous + interval * (w6 - w10);

    public static double W10(
        double interval,
        double w6Rate,
        double T6,
        double level) => SystemDynamics.GetDelay(
            level,
            w6Rate,
            T6,
            interval,
            3).Rate;

    public static double X5(
        double previous,
        double interval,
        double w10,
        double w11) => previous + interval * (w10 - w11);

    public static double W11(
        double interval,
        double w10Rate,
        double T7,
        double level) => SystemDynamics.GetDelay(level,
            w10Rate,
            T7,
            interval,
            3).Rate;

    public static double X6(
        double previous,
        double interval,
        double f3,
        double w1) => previous + interval * (f3 - w1);

    public static double W1(
        double interval,
        double f3Rate,
        double T8,
        double level) => SystemDynamics.GetDelay(
            level,
            f3Rate,
            T8,
            interval,
            3).Rate;
}
