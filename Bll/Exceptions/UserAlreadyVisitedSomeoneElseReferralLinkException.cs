namespace Bll.Exceptions;

public class UserAlreadyVisitedSomeoneElseReferralLinkException : Exception
{
    public UserAlreadyVisitedSomeoneElseReferralLinkException(string message) 
        : base(message) { }
    
    public UserAlreadyVisitedSomeoneElseReferralLinkException(string message, Exception innerException)
        : base(message, innerException) { }
}