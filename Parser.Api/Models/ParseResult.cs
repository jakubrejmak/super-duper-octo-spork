using System.Text.Json;

namespace Parser.Api.Models;

public record ParseResult(int Count, IReadOnlyList<IReadOnlyDictionary<string, object?>> Objects);
