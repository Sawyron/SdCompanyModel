using CommunityToolkit.Mvvm.Messaging.Messages;
using Infrastructure.Solution;

namespace WpfUi.Solution.Messages;

public class DrawSolutionMessage(SystemSolutionResult solution) : ValueChangedMessage<SystemSolutionResult>(solution)
{ }
