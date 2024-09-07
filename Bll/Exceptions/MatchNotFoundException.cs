namespace Bll.Exceptions;

public class MatchNotFoundException : Exception
{
    public MatchNotFoundException(string message) 
        : base(message) { }
    
    public MatchNotFoundException(string message, Exception ex)
        : base(message, ex) { }
}