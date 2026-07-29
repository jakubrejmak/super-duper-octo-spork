namespace Parser.Api.Dtos.v1;

public record ParseResponse(
    int Count,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Objects
);
