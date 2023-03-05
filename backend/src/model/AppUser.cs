using System.Reflection.Metadata.Ecma335;

namespace TeacherPractise.Model
{
  public class AppUser
  {
    public AppUser(string email, string passwordHash)
    {
      Email = email;
      PasswordHash = passwordHash;
    }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public List<string> Roles { get; private set; } = new();
  }
}