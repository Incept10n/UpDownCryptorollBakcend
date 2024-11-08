namespace Bll.Exceptions;

public class InvalidReferralSalt : Exception
{
    public InvalidReferralSalt(string message) 
        : base(message) { }
    
    public InvalidReferralSalt(string message, Exception inner)
        : base(message, inner) { }
}