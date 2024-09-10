namespace Bll.Exceptions;

public class UserAlreadyInMatchException : Exception
{
    public UserAlreadyInMatchException(string message)
        : base(message) { }
    public UserAlreadyInMatchException(string message, Exception ex)
        : base(message, ex) { }
}