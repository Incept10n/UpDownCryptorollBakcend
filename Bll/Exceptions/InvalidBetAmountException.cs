namespace Bll.Exceptions;

public class InvalidBetAmountException : Exception
{
    public InvalidBetAmountException(string message)
        : base(message) { }
    
    public InvalidBetAmountException(string message, Exception ex) 
        : base(message, ex) { }
}