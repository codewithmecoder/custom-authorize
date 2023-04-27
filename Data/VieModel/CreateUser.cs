namespace CustomAuthorize.Data.VieModel;

public class CreateUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }

    public CreateUser(string name, string username, string email)
    {
        Name = name;
        Username = username;
        Email = email;
    }
}