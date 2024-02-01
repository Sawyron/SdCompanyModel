namespace Solution.Common;
public interface IStepResolver
{
    IDictionary<string, double> ResolveStep(IReadOnlyDictionary<string, double> variables, double interval);
}
