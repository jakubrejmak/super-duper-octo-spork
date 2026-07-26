namespace Parser.Api.Parsing;

using Parser.Api.Exceptions;
using Parser.Api.Models;
using nietras.SeparatedValues;

public class SepCSVParser : IContentParser
{
    public ContentType Type => ContentType.CSV;

    public ParseResult ParseToDictList(Stream stream)
    {
        using var reader = Sep.Reader().From(stream);

        if (!reader.HasHeader)
        {
            throw new ParseException("CSV has to have a header");
        }
        var header = reader.Header;

        List<IReadOnlyDictionary<string, object?>> list = new();

        int count = 0;
        foreach (var row in reader)
        {
            list.Add(ToDictionary(row, header));
            count++;
        }

        return new ParseResult(count, list);
    }

    IReadOnlyDictionary<string, object?> ToDictionary(SepReader.Row row, SepReaderHeader header)
    {
        Dictionary<string, object?> dict = new();

        foreach (var col in header.ColNames)
        {
            dict[col] = row[col].ToString();
        }

        return dict;
    }
}
