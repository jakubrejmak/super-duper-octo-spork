using System.Text;
using Parser.Api.Exceptions;
using Parser.Api.Parsing;

namespace Parser.Tests.Parsing;

public class JSONParserTests
{
    [Fact]
    public void ParseToDictList_CorrectJSON_ProducesCorrectOutput()
    {
        const string jsonString = """
            [
              {
                "string": "Home",
                "number": 3,
                "decimal": 3.99,
                "boolean": true,
                "date": "2023-08-17T08:20:28.438Z"
              },
              {
                "nested": {
                  "parsed": true
                },
                "array": [1, 2, 3]
              },
              {
                "empty": {}
              }
            ]
            """;

        var parser = new JSONParser();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

        var parsed = parser.ParseToDictList(stream);

        Assert.Equal(3, parsed.Count);

        Assert.Equal("Home", parsed.Objects[0]["string"]);
        Assert.Equal(3L, parsed.Objects[0]["number"]);
        Assert.Equal(3.99M, parsed.Objects[0]["decimal"]);
        Assert.Equal(true, parsed.Objects[0]["boolean"]);
        Assert.Equal(
            DateTime.Parse("2023-08-17T08:20:28.438Z").ToUniversalTime(),
            parsed.Objects[0]["date"]
        );

        var nested = Assert.IsAssignableFrom<IReadOnlyDictionary<string, object?>>(
            parsed.Objects[1]["nested"]
        );
        Assert.Equal(true, nested["parsed"]);

        var array = Assert.IsAssignableFrom<IReadOnlyList<object?>>(parsed.Objects[1]["array"]);
        Assert.Equal(3, array.Count);
        Assert.Equal(1L, array[0]);
        Assert.Equal(2L, array[1]);
        Assert.Equal(3L, array[2]);

        var empty = Assert.IsAssignableFrom<IReadOnlyDictionary<string, object?>>(
            parsed.Objects[2]["empty"]
        );
        Assert.Empty(empty);
    }

    [Theory]
    [InlineData(
        """
            {
              "string": "Home",
              "number": 3,
            }
            """
    )]
    [InlineData(
        """
            {
              "string": "Home",
              "number": 3
            """
    )]
    [InlineData("this is not json")]
    [InlineData("")]
    [InlineData("{")]
    [InlineData("[}")]
    [InlineData("{ invalid json }")]
    public void ParseToDictList_InvalidJSON_ThrowsParseException(string jsonString)
    {
        var parser = new JSONParser();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

        Assert.Throws<ParseException>(() => parser.ParseToDictList(stream));
    }
}
