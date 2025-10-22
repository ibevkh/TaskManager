using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Core.DTOs;

public class ProblemDetailsDto : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}