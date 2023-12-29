using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WpfUi.Solution.Messages;

public class DrawSolutionMessage(double[] time, IDictionary<string, double[]> values)
    : ValueChangedMessage<(double[] Time, IDictionary<string, double[]> Values)>((time, values))
{ }
