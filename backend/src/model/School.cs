namespace teacherpractise.model
{
    public class School
    {
        private int Id { get; set; }
        private string? name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}