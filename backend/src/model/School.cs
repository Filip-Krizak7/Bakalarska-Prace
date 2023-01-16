using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace teacherpractise.model
{
    public class School
    {
        private int Id { get; set; }
        [Required]
        private string? name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}