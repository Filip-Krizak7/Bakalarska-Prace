using System;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherPractise.Model;
using TeacherPractise.Config;

namespace TeacherPractise.Service.Token.PasswordResetToken
{
    public class PasswordResetToken
    {
        private const int EXPIRATION = AppConfig.FORGOT_PASSWORD_TOKEN_EXPIRY_TIME;

        public long Id { get; set; }
        public string Token { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime ExpiryDate { get; set; }

        public PasswordResetToken()
        {
            // Empty constructor required for entity framework
        }

        public PasswordResetToken(string token)
        {
            Token = token;
            ExpiryDate = calculateExpiryDate(EXPIRATION);
        }

        public PasswordResetToken(string token, int userId)
        {
            Token = token;
            UserId = userId;
            ExpiryDate = calculateExpiryDate(EXPIRATION);
        }
        
        public PasswordResetToken(string token, User user)
        {
            Token = token;
            User = user;
            ExpiryDate = calculateExpiryDate(EXPIRATION);
        }

        private DateTime calculateExpiryDate(int expiryTimeInMinutes)
        {
            var cal = DateTime.Now;
            cal = cal.AddMinutes(expiryTimeInMinutes);
            return cal;
        }

        public void updateToken(string token)
        {
            Token = token;
            ExpiryDate = calculateExpiryDate(EXPIRATION);
        }
    }
}