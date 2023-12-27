using Infrastructure.Csv;
using Solution.Conditions;
using Solution.SolutionProviders;
using System.Globalization;

namespace Infrastructure.Solution;

public class SolutionService
{
    public SystemSolutionResult GetSolution(double interval, double end, SystemSolutionProvider solutionProvider)
    {
        List<SolutionStep> result = solutionProvider.GetSolution(interval, end).ToList();
        return new SystemSolutionResult(result);
    }

    public async Task WriteToStreamAsCsvAsync(SystemSolutionResult solutionResult, Stream stream)
    {
        var headers = solutionResult.Steps.First()
            .Values.Keys
            .OrderBy(k => k)
            .Prepend("time")
            .ToArray();
        var values = solutionResult.Steps.Select(step =>
             headers[1..].Select(h => step.Values[h])
                .Prepend(step.Time)
                .Select(value => value.ToString(CultureInfo.InvariantCulture))
                .ToArray())
            .ToArray();
        var csv = new CsvDto(headers, values);
        var csvWriter = new CsvWriter();
        await csvWriter.WriteToStreamAsync(csv, stream);
    }
}
