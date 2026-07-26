using Parser.Api.Models;

namespace Parser.Api.Parsing;

public interface IContentParser
{
    ContentType Type { get; }

    ParseResult ParseToDictList(Stream stream);
}
