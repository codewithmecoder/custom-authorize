using System.ComponentModel.DataAnnotations.Schema;

namespace CustomAuthorize.Data;

public class RoleModel
{
    [Column("id")] public int Id { get; set; }
    [Column("user_id")] public int UserModelId { get; set; }
    [Column("name")] public string Name { get; set; }

    public RoleModel(int id, int userModelId, string name)
    {
        Id = id;
        UserModelId = userModelId;
        Name = name;
    }
}