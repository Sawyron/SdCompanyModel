namespace Infrastructure.Csv;

public class CsvWriter
{
    public string Delimiter { get; init; } = ",";
    public async Task WriteToStreamAsync(CsvDto csv, Stream stream)
    {
        if (ValidateCsv(csv) is Exception exception)
        {
            throw exception;
        }
        async Task WriteLineAsync(string[] line, StreamWriter writer)
        {
            foreach (string value in line[..^1])
            {
                await writer.WriteAsync(value);
                await writer.WriteAsync(Delimiter);
            }
            await writer.WriteLineAsync(line[^1]);
        }
        var writer = new StreamWriter(stream);
        await WriteLineAsync(csv.Headers, writer);
        foreach (string[] line in csv.Lines)
        {
            await WriteLineAsync(line, writer);
        }
    }

    private static Exception? ValidateCsv(CsvDto csv)
    {
        var lengthsAreNotEqualException = new ArgumentException("Lines length should be equal to header's");
        if (csv.Lines.Length == 0 && csv.Headers.Length > 0)
        {
            return lengthsAreNotEqualException;
        }
        if (csv.Headers.Length > 0 && csv.Lines.Length == 0)
        {
            return lengthsAreNotEqualException;
        }
        if (csv.Headers.Length != csv.Lines[0].Length)
        {
            return lengthsAreNotEqualException;
        }
        return null;
    }
}
