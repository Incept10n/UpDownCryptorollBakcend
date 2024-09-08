namespace Bll.Exceptions;

public class WrongPredictionTimeframeException : Exception
{
    public WrongPredictionTimeframeException(string message) 
        : base(message) { }
        
    public WrongPredictionTimeframeException(string message, Exception ex)
        : base(message, ex) { }
}