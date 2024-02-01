namespace Solution.Common;
public class SolutionResolver
{
    private readonly IStepResolver _stepResolver;
    private readonly Func<IDictionary<string, double>> _initialConditionsFactory;

    public SolutionResolver(IStepResolver stepResolver, Func<IDictionary<string, double>> initialConditionsFactory)
    {
        _stepResolver = stepResolver;
        _initialConditionsFactory = initialConditionsFactory;
    }

    public IEnumerable<SolutionStep> GetSolution(double start, double end, double interval)
    {
        int steps = Convert.ToInt32((end - start) / interval) + 1;
        IDictionary<string, double> currentStep = _stepResolver.ResolveStep(
            _initialConditionsFactory().AsReadOnly(),
            interval);
        yield return new SolutionStep(0, currentStep);
        for (int i = 1; i < steps; i++)
        {
            IDictionary<string, double> nextStep = _stepResolver.ResolveStep(currentStep.AsReadOnly(), interval);
            yield return new SolutionStep(i * interval, nextStep);
            currentStep = nextStep;
        }
    }
}
