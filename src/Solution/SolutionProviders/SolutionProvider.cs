using Solution.Conditions;

namespace Solution.SolutionProviders;

public abstract class SolutionProvider
{
    public abstract IDictionary<string, double> GetFirstStep();
    public abstract IDictionary<string, double> ResolveStep(
        IDictionary<string, double> previousStep,
        IDictionary<string, double> externals,
        double interval);

    public IEnumerable<SolutionStep> GetSolution(double start, double end) =>
        GetSolution(start, end, () => new Dictionary<string, double>());

    public IEnumerable<SolutionStep> GetSolution(
        double interval,
        double end,
        Func<IDictionary<string, double>> externalsFactory)
    {
        int steps = Convert.ToInt32(end / interval) + 1;
        IDictionary<string, double> currentStep = GetFirstStep();
        yield return new SolutionStep(0, currentStep);
        for (int i = 1; i < steps; i++)
        {
            IDictionary<string, double> nextStep = ResolveStep(currentStep, externalsFactory(), interval);
            yield return new SolutionStep(i * interval, nextStep);
            currentStep = nextStep;
        }
    }
}
