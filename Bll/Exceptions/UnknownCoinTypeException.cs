namespace Bll.Exceptions;

public class UnknownCoinTypeException : Exception
{
    public UnknownCoinTypeException(string message)
        : base(message) { }
    
    public UnknownCoinTypeException(string message, Exception exception)
        : base(message, exception) { } 
}