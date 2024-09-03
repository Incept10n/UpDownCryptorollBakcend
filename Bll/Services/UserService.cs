using AutoMapper;
using Bll.Dtos;
using Dal.DatabaseContext;
using Dal.Entities;

namespace Bll.Services;

public class UserService(
    ApplicationDbContext applicationDbContext,
    IMapper mapper)
{
    public UserDto GetUserInfo(string walletAddress)
    {
        var user =  applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is not null) return mapper.Map<UserDto>(user);
        
        var newUser = new User
        {
            WalletAddress = walletAddress,
            CurrentBalance = 0,
            LoginStreakCount = 0,
            Name = "Unknown player"
        };

        applicationDbContext.Users.Add(newUser);
        applicationDbContext.SaveChanges();

        return mapper.Map<UserDto>(newUser);
    }

    public bool ChangeUserName(string walletAddress, UserChangeNameDto userChangeNameDto)
    {
        var user = applicationDbContext.Users.FirstOrDefault(user => user.WalletAddress == walletAddress);

        if (user is null)
        {
            return false;
        }

        user.Name = userChangeNameDto.newName;

        applicationDbContext.SaveChanges();

        return true;
    }
}