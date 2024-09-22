namespace Skill.Integration.Models;
public class Token
{
    public string Access_Token { get; set; } = null!;
    public int Expires_In { get; set; }
    public string Token_Type { get; set; } = null!;
    public string Scope {  get; set; } = null!;
}

