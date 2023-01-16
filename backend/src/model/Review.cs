using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace teacherpractise.model
{
    public class Review
    {
        private int Id { get; set; }

        private int userId { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }

        private int practiceId { get; set; }
        [ForeignKey("practiceId")]
        public Practice Practice { get; set; }

        [Required]
        private string? text { get; set; }
    }
}