using Parser.Api.Models;
using Parser.Api.Parsing;

namespace Parser.Api.Services;

public class ParserService
{
    private readonly Dictionary<ContentType, IContentParser> _parsers;

    public ParserService(IEnumerable<IContentParser> parsers)
    {
        _parsers = parsers.ToDictionary(p => p.Type);
    }

    public ParseResult Parse(ContentType type, Stream stream)
    {
        var parser = _parsers[type];

        return parser.ParseToDictList(stream);
    }
}
