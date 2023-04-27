using System.ComponentModel.DataAnnotations.Schema;

namespace CustomAuthorize.Data;

public class UserModel
{
    [Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    [Column("email")] public string Email { get; set; }

    [Column("username")] public string Username { get; set; }

    public List<RoleModel> Roles { get; set; } = new List<RoleModel>();

    public UserModel(int id, string name, string email, string username)
    {
        Id = id;
        Name = name;
        Email = email;
        Username = username;
    }
}