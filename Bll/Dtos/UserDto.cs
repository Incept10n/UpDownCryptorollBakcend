namespace Bll.Dtos;

public class UserDto
{
    public string WalletAddress { get; set; }
    public string Name { get; set; }
    public int CurrentBalance { get; set; }
    public int LoginStreakCount { get; set; }
    
    public bool IsLastMatchCollected { get; set; }
}