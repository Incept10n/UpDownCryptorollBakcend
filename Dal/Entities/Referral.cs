namespace Dal.Entities;

public class Referral
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ReferralSalt { get; set; }
    public int FriendsInvited { get; set; }
    public bool VisitedOtherUserReferralLink { get; set; }
}