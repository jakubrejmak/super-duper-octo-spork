using System.Text;
using Parser.Api.Exceptions;
using Parser.Api.Parsing;

namespace Parser.Tests.Parsing;

public class SepCSVParserTests
{
    [Fact]
    public void ParseToDictList_ValidCsv_ReturnsExpectedResult()
    {
        const string csvString = """
            A,B,C,D
            1,2,3,4
            5,6,7,8
            """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvString));
        var parser = new SepCSVParser();

        var result = parser.ParseToDictList(stream);

        Assert.Equal(2, result.Count);

        var expected1 = new Dictionary<string, object?>
        {
            ["A"] = "1",
            ["B"] = "2",
            ["C"] = "3",
            ["D"] = "4",
        };

        Assert.Equal(expected1, result.Objects[0]);

        var expected2 = new Dictionary<string, object?>
        {
            ["A"] = "5",
            ["B"] = "6",
            ["C"] = "7",
            ["D"] = "8",
        };

        Assert.Equal(expected2, result.Objects[1]);
    }

    [Theory]
    [InlineData("")]
    public void ParseToDictList_CSV_WithoutHeader_ThrowsException(string csvString)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvString));
        var parser = new SepCSVParser();

        Assert.Throws<ParseException>(() => parser.ParseToDictList(stream));
    }

    [Theory]
    [InlineData(
        """
            A,C,D
            1,2,3,4
            5,6,8,4
            """
    )]
    [InlineData(
        """
            A,B,C,D
            1,2,4
            5,6,8,4
            """
    )]
    [InlineData(
        """
            A,B,C,D
            1,2,3,4,5
            5,6,8,4
            """
    )]
    public void ParseToDictList_CSV_Malformed_ThrowsException(string csvString)
    {
        var parser = new SepCSVParser();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvString));

        Assert.Throws<ParseException>(() => parser.ParseToDictList(stream));
    }
}
