using System.Net;

namespace Domain.Exceptions;

public class AppNotFoundException(List<string> errors) : AppBaseException(string.Empty)
{
    private readonly List<string> _errors = errors;

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return _errors;
    }
}