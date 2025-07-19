namespace Domain.Exceptions;

public abstract class AppBaseException(string message) : SystemException(message)
{
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}