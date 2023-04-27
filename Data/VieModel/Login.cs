namespace CustomAuthorize.Data.VieModel;

public class Login
{
    public string Username { get; set; }

    public Login(string username)
    {
        Username = username;
    }
}