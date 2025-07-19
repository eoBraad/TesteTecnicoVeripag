namespace Domain.Responses;

public class ResponseErrorJson(List<string> errors)
{
    public List<string> Errors { get; } = errors;
}