namespace Bll.Exceptions;

public class UserCannotVisitHisOwnReferralLink : Exception
{
    public UserCannotVisitHisOwnReferralLink(string message)
        : base(message) { }
    
    public UserCannotVisitHisOwnReferralLink(string message, Exception inner)
        : base(message, inner) { }
}