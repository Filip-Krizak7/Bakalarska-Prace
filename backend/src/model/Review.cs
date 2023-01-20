using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class Review
    {
        private int Id { get; set; }

        private int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        private int PracticeId { get; set; }
        [ForeignKey("PracticeId")]
        public Practice Practice { get; set; }

        [Required]
        private string? Text { get; set; }
    }
}