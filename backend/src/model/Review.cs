using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int PracticeId { get; set; }
        [ForeignKey("PracticeId")]
        public Practice Practice { get; set; }

        [Required]
        public string Text { get; set; }

        public Review(int userId, int practiceId, string text)
        {
            this.UserId = userId;
            this.PracticeId = practiceId;
            this.Text = text;
        }

        public Review()
        {}
    }
}