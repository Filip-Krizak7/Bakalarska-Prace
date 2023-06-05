using System;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherPractise.Model;

namespace TeacherPractise.Service.Token.RegistrationToken
{
    public class ConfirmationToken
    {
        [ForeignKey("User")]
        public int ConfirmationTokenId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public virtual User User { get; set; }

        public ConfirmationToken(int confirmationTokenId, string token, DateTime createdAt, DateTime expiresAt)
        {
            ConfirmationTokenId = confirmationTokenId;
            Token = token;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }

        public ConfirmationToken()
        {}
    }
}