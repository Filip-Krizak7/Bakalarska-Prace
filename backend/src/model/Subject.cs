using System.ComponentModel.DataAnnotations;

namespace teacherpractise.model
{
    public class Subject
    {
        private int Id { get; set; }
        [Required]
        private string? name { get; set; }

        public virtual ICollection<Practice> Practices { get; set; }
    }
}