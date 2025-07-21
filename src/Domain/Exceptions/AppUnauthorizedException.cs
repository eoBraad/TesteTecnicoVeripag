using System.Net;

namespace Domain.Exceptions;

public class AppUnauthorizedException(List<string> errors) : AppBaseException(string.Empty)
{
    private readonly List<string> _errors = errors;

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return _errors;
    }
}