namespace Bll.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message)
        : base(message)  { }

    public UserNotFoundException(string message, Exception ex)
        : base(message, ex) { }
}