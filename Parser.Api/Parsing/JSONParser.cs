namespace Parser.Api.Parsing;

using System.Text.Json;
using Parser.Api.Exceptions;
using Parser.Api.Models;

public class JSONParser : IContentParser
{
    public ContentType Type => ContentType.INTERNAL_JSON;

    public ParseResult ParseToDictList(Stream stream)
    {
        using var document = JsonDocument.Parse(stream);

        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Array)
        {
            throw new ParseException($"Object must be a JSON Array. Got {root.ValueKind}");
        }

        var list = new List<IReadOnlyDictionary<string, object?>>();

        foreach (var jsonObject in root.EnumerateArray())
        {
            list.Add(ToDictionary(jsonObject));
        }

        return new ParseResult(root.GetArrayLength(), list);
    }

    private object? ConvertValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Undefined or JsonValueKind.Null => null,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number when element.TryGetInt64(out long l) => l,
            JsonValueKind.Number => element.GetDecimal(),
            JsonValueKind.String when element.TryGetDateTime(out DateTime datetime) => datetime,
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Array => ToList(element),
            JsonValueKind.Object => ToDictionary(element),
            _ => throw new ParseException($"Unsuported JsonValueKind: {element.ValueKind}"),
        };
    }

    private IReadOnlyList<object?> ToList(JsonElement arr)
    {
        List<object?> list = new();

        foreach (var element in arr.EnumerateArray())
            list.Add(ConvertValue(element));

        return list;
    }

    private IReadOnlyDictionary<string, object?> ToDictionary(JsonElement obj)
    {
        Dictionary<string, object?> dict = new();

        foreach (var prop in obj.EnumerateObject())
            dict[prop.Name] = ConvertValue(prop.Value);
        return dict;
    }
}
