using System;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherPractise.Model;

namespace TeacherPractise.Service.Token.RegistrationToken
{
    public class ConfirmationToken
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public virtual User AppUser { get; set; }

        public ConfirmationToken(string token, DateTime createdAt, DateTime expiresAt, User appUser)
        {
            Token = token;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            AppUser = appUser;
        }
    }
}