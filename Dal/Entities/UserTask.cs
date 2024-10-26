namespace Dal.Entities;

public class UserTask
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User.User User { get; set; }

    public string Completed { get; set; }
    public string Uncollected { get; set; }
    public string Uncompleted { get; set; }
}