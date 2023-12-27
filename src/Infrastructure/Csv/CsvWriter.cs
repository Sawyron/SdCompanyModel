namespace Infrastructure.Csv;

public class CsvWriter
{
    public async Task WriteToStreamAsync(CsvDto csv, Stream stream, string delimiter = ",")
    {
        
        if (csv.Headers.Length != csv.Lines[0].Length)
        {
            throw new ArgumentException("Lines length should be equal to header's");
        }
        async Task WriteLineAsync(string[] line, StreamWriter writer)
        {
            foreach(string value in line[..(line.Length -1)])
            {
                await writer.WriteAsync(value);
                await writer.WriteAsync(delimiter);
            }
            await writer.WriteLineAsync();
        }
        var writer = new StreamWriter(stream);
        await WriteLineAsync(csv.Headers, writer);
        foreach (string[] line in csv.Lines)
        {
            await WriteLineAsync(line, writer);
        }
    }
}
