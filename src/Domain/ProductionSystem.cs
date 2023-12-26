namespace Domain;

public static class ProductionSystem
{
    public static double Y1(
        double previous,
        double interval,
        double w11,
        double f3) => previous + interval * (w11 - f3);

    public static double Y2(
        double previous,
        double interval,
        double v1,
        double f3) => previous + interval * (v1 - f3);

    public static double V2(double y1, double v3) => y1 / v3;

    public static double V4(double interval, double y2) => y2 / interval;

    public static double F3(double v2, double v4) => Math.Min(v2, v4);

    public static double V3(
        double t2,
        double t3,
        double v5,
        double y2) => t2 + t3 * v5 / y2;

    public static double V5(double k, double y3) => k * y3;

    public static double Y3(
        double previous,
        double interval,
        double t4,
        double w11) => (1 - interval / t4) * previous + (interval / t4) * w11;

    public static double V6(
        double y1,
        double y2,
        double t5,
        double v5,
        double v7,
        double v8,
        double v9,
        double w11) => w11 + (1 / t5) * (v5 - y2 + v7 - v8 + y1 - v9);

    public static double V11(double v6, double beta) => Math.Min(v6, beta);

    public static double V7(double t6, double t7, double y3) => (t6 + t7) * y3;

    public static double V8(double y4, double y5) => y4 + y5;

    public static double V9(
        double t2,
        double t7,
        double y3) => (t2 + t7) * y3;

    public static double Y4(
        double previous,
        double interval,
        double v6,
        double v10) => previous + interval * (v6 - v10);

    public static double V10(
        double interval,
        double v6Rate,
        double t6,
        double level) => SystemDynamics.GetDelay(
            level,
            v6Rate,
            t6,
            interval,
            3).Rate;

    public static double Y5(
        double previous,
        double interval,
        double v10,
        double v1) => previous + interval * (v10 - v1);

    public static double V1(
        double interval,
        double v10Rate,
        double t7,
        double level) => SystemDynamics.GetDelay(
            level,
            v10Rate,
            t7,
            interval,
            3).Rate;
}
