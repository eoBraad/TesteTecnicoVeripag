using System.Net;

namespace Domain.Exceptions;

public class AppValidationException(List<string> errors) : AppBaseException(string.Empty)
{
    private readonly List<string> _errors = errors;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return _errors;
    }
}