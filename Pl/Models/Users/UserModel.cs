namespace UpDownCryptorollBackend.Models.Users;

public class UserModel
{
    public string WalletAddress { get; set; }
    public string Username { get; set; }
    public int CurrentBalance { get; set; }
    public int LoginStreakCount { get; set; }
    public bool IsLastMatchCollected { get; set; }
}