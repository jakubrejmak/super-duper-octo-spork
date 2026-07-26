using System.Text.Json.Serialization;
using Parser.Api.Endpoints.v1;
using Parser.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(allowIntegerValues: false)
    );
});

builder.Services.Configure<RouteHandlerOptions>(options =>
{
    options.ThrowOnBadRequest = false;
});

builder.Services.AddParserServices();


var app = builder.Build();

var api = app.MapGroup("/api");
var v1 = api.MapGroup("/v1").WithTags("v1");

v1.MapParserEndpoints();

app.Run();
