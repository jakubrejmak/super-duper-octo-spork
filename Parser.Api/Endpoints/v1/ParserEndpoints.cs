namespace Parser.Api.Endpoints.v1;

using Parser.Api.Dtos.v1;
using Parser.Api.Services;

public static class ParserEndpoints
{
    public static RouteGroupBuilder MapParserEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost(
            "/parse-content",
            (ParseRequest payload, ParserService parser) =>
            {
                var bytes = Convert.FromBase64String(payload.Content);
                var stream = new MemoryStream(bytes);

                var result = parser.Parse(payload.Type!.Value, stream);

                return new ParseResponse("success", result.Count, result.Objects);
            }
        );

        return group;
    }
}
