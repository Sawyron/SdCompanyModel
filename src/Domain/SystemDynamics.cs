namespace Domain;

public static class SystemDynamics
{
    public static (double Level, double Rate) GetDelay(
        double inputLevel,
        double inputRate,
        double delayValue,
        double interval,
        int order)
    {
        double GetRate(double level) => level / delayValue / order;
        double GetLevel(double previous, double inputRate, double outputRate) =>
            previous + interval * (inputRate - outputRate);
        double rate = GetRate(inputLevel);
        double level = GetLevel(inputLevel, inputRate, rate);
        for (int i = 1; i < order; i++)
        {
            double currentRate = rate;
            rate = GetRate(level);
            level = GetLevel(level, currentRate, rate);
        }
        return (level, rate);
    }
}
