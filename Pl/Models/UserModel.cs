namespace UpDownCryptorollBackend.Models;

public class UserModel
{
    public string Username { get; set; }
    public int CurrentBalance { get; set; }
    public int LoginStreakCount { get; set; }
    public bool IsLastMatchCollected { get; set; }
}