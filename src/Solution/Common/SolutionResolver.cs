﻿namespace Solution.Common;
public class SolutionResolver
{
    private readonly Func<IStepResolver> _stepResolverFactory;
    private readonly Func<IDictionary<string, double>> _initialConditionsFactory;

    public SolutionResolver(
        Func<IStepResolver> stepResolver,
        Func<IDictionary<string, double>> initialConditionsFactory)
    {
        _stepResolverFactory = stepResolver;
        _initialConditionsFactory = initialConditionsFactory;
    }

    public IEnumerable<SolutionStep> GetSolution(double start, double end, double interval)
    {
        int steps = Convert.ToInt32((end - start) / interval) + 1;
        var stepResolver = _stepResolverFactory();
        IDictionary<string, double> currentStep = stepResolver.ResolveStep(
            _initialConditionsFactory().AsReadOnly(),
            interval);
        yield return new SolutionStep(0, currentStep);
        for (int i = 1; i < steps; i++)
        {
            IDictionary<string, double> nextStep = stepResolver.ResolveStep(currentStep.AsReadOnly(), interval);
            yield return new SolutionStep(i * interval, nextStep);
            currentStep = nextStep;
        }
    }
}
