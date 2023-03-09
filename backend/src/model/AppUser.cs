using System.Reflection.Metadata.Ecma335;

namespace TeacherPractise.Model
{
  public class AppUser
  {
    public AppUser(string username, string passwordHash)
    {
      Username = username;
      PasswordHash = passwordHash;
    }

    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public List<string> Roles { get; private set; } = new();
  }
}