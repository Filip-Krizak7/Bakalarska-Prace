namespace teacherpractise.model
{
    public class Subject
    {
        private int Id { get; set; }
        private string? name { get; set; }

        public virtual ICollection<Practice> Practices { get; set; }
    }
}