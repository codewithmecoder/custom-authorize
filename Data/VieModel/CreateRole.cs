namespace CustomAuthorize.Data.VieModel;

public class CreateRole
{
    public int UserModelId { get; set; }
    public string Name { get; set; }

    public CreateRole(int userModelId, string name)
    {
        UserModelId = userModelId;
        Name = name;
    }
}