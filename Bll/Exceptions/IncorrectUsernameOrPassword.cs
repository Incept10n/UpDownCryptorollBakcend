namespace Bll.Exceptions;

public class IncorrectUsernameOrPassword : Exception
{
    public IncorrectUsernameOrPassword(string message) 
        : base(message) {} 
    
    public IncorrectUsernameOrPassword(string message, Exception inner)
        : base(message, inner) {}
}