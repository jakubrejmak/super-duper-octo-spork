namespace Parser.Api.Extensions;

using Parser.Api.Middleware;
using Parser.Api.Parsing;
using Parser.Api.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParserServices(this IServiceCollection services)
    {
        services.AddSingleton<ParserService>();
        services.AddSingleton<IContentParser, SepCSVParser>();
        services.AddSingleton<IContentParser, JSONParser>();
        services.AddExceptionHandler<ParseExceptionHandler>();

        return services;
    }
}
