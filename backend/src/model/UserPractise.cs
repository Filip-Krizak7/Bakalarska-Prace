namespace teacherpractise.model
{
    public class UserPractice
    {
        private int practiceId { get; set; }
        private int userId { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}