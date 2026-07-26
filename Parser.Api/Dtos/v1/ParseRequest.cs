using System.ComponentModel.DataAnnotations;
using Parser.Api.Models;

namespace Parser.Api.Dtos.v1;

public record ParseRequest(
    [Required] ContentType? Type,
    [Required] [Base64String] string Content // base64 encoded
);
