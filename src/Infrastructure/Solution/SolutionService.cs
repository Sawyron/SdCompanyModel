using Infrastructure.Csv;
using Solution.Common;
using Solution.Company;
using System.Globalization;

namespace Infrastructure.Solution;

public class SolutionService
{
    private readonly SolutionResolver _solutionResolver;

    public SolutionService(SolutionResolver solutionResolver)
    {
        _solutionResolver = solutionResolver;
    }

    public SystemSolutionResult GetSolution(double interval, double end)
    {
        var result = _solutionResolver.GetSolution(0, end, interval);
        return new SystemSolutionResult(result.ToList());
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
