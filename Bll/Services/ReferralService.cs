using System.Security.Cryptography;
using System.Text;
using Bll.Dtos.Tasks;
using Bll.Exceptions;
using Dal.DatabaseContext;
using Dal.Entities;
using Dal.Entities.User;
using Dal.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bll.Services;

public class ReferralService(
    ApplicationDbContext dbContext,
    UserService userService,
    RewardTaskService rewardTaskService)
{
    public async Task<string> GetUserReferralSaltAsync(string username)
    {
        var user = userService.GetUserByName(username);

        var referralLinkSalt = await dbContext.Referrals.FirstOrDefaultAsync(r => r.UserId == user.Id);

        if (referralLinkSalt is not null) return referralLinkSalt.ReferralSalt;
        
        var newReferral = CreateNewReferralEntity(user);
        dbContext.Referrals.Add(newReferral);
        await dbContext.SaveChangesAsync();

        return newReferral.ReferralSalt;
    }

    public async Task VisitReferralAsync(string visitorName, string referralSalt)
    {
        var visitor = userService.GetUserByName(visitorName);
        var visitorReferral = dbContext.Referrals.FirstOrDefault(r => r.UserId == visitor.Id);
        
        var referral = dbContext.Referrals.FirstOrDefault(r => r.ReferralSalt == referralSalt);

        if (referral is null) 
            throw new InvalidReferralSalt($"Referral salt {referralSalt} is invalid");
        if (visitor.Id == referral.UserId)
            throw new UserCannotVisitHisOwnReferralLink("user cannot visit his own referral link");

        if (visitorReferral is null)
        {
            visitorReferral = CreateNewReferralEntity(visitor);
            dbContext.Referrals.Add(visitorReferral);
        }
        else if (visitorReferral.VisitedOtherUserReferralLink)
        {
            throw new UserAlreadyVisitedSomeoneElseReferralLinkException(
                $"user with name {visitor.Name} has already visited referral link");
        }

        referral.FriendsInvited += 1;
        visitorReferral.VisitedOtherUserReferralLink = true;

        switch (referral.FriendsInvited)
        {
            case 10:
            {
                var referralLinkUser = dbContext.Users.First(u => u.Id == referral.UserId);
            
                rewardTaskService.ChangeTaskStatus(referralLinkUser.Name, new RewardTaskChangeDto
                {
                    TaskId = 4,
                    ChangedStatus = RewardTaskStatus.Uncollected,
                });
                break;
            }
            case > 10:
                referral.FriendsInvited = 10;
                break;
        }

        await dbContext.SaveChangesAsync();
    }

    private Referral CreateNewReferralEntity(User user)
    {
        return new Referral
        {
            UserId = user.Id,
            FriendsInvited = 0,
            VisitedOtherUserReferralLink = false,
            ReferralSalt = GenerateRandomSalt(user.Id, user.Name),
        };
    }

    private string GenerateRandomSalt(int userId, string username)
    {
        var input = $"{userId}-{username}";

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        string base64Hash = Convert.ToBase64String(bytes)
            .Replace("=", "")
            .Replace("+", "")
            .Replace("/", "");

        return base64Hash.Substring(0, 16);
    }
}