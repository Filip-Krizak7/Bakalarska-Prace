using TeacherPractise.Model;
using TeacherPractise.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TeacherPractise.Service
{
  public class SecurityService
  {
    private readonly IConfiguration configuration;
    public byte[] Key { get; } = RandomNumberGenerator.GetBytes(128);

    public SecurityService([FromServices] IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public string BuildJwtToken(User appUser)
    {
      // key from configuration:
      // ... or unique key per app start
      var key = this.Key;
      //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

      List<string> roleList = new List<string>();
      roleList.Add(appUser.Role.ToString());

      Dictionary<string, object> roleClaims = roleList.ToDictionary(
          q => ClaimTypes.Role,
          q => (object)q.ToUpper());

      try
      {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(new[]
            {
          new Claim(JwtRegisteredClaimNames.Sub, appUser.Username),
          new Claim(JwtRegisteredClaimNames.Email, appUser.Username),
          new Claim(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
          new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"]),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
          Expires = DateTime.UtcNow.AddSeconds(AppConfig.TOKEN_EXPIRATION_IN_SECONDS),
          SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature),
          Claims = roleClaims
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var ret = tokenHandler.WriteToken(token);
        return ret;
      }
      catch(NullReferenceException ex)
      {
        Console.WriteLine(ex);
        Console.WriteLine("Došlo k výjimce NullReferenceException: {0}", ex.Message);
      }

      return "fail";
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
      bool ret;
      ret = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
      return ret;
    }

    public string HashPassword(string password)
    {
      string ret = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
      return ret;
    }
  }
}