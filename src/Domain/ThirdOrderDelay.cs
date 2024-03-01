namespace Domain;
public class ThirdOrderDelay
{
    private readonly double _delayValue;
    private readonly double[] _levels = new double[3];

    public ThirdOrderDelay(double delayValue)
    {
        _delayValue = delayValue;
    }

    public (double Level, double Rate) GetDelayValues(double interval, double input)
    {
        double rate = input;
        for (int i = 0; i < _levels.Length; i++)
        {
            double currentRate = _levels[i] / (_delayValue / 3);
            _levels[i] = _levels[i] + interval * (rate - currentRate);
            rate = currentRate;
        }
        return (_levels.Sum(), rate);
    }

    public void Reset() => Array.Fill(_levels, 0);
}
