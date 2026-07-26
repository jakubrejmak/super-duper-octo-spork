using System.Text.Json;

namespace Parser.Api.Dtos.v1;

public record ParseResponse(string Status, int Count, IReadOnlyList<IReadOnlyDictionary<string, object?>> Objects);
