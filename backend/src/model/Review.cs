using System.ComponentModel.DataAnnotations;

namespace teacherpractise.model
{
    public class Review
    {
        private int Id { get; set; }

        private int userId { get; set; }
        [Required]
        public User User { get; set; }

        [Required]
        private int practiceId { get; set; }
        [Required]
        public Practice Practice { get; set; }

        [Required]
        private string? text { get; set; }
    }
}