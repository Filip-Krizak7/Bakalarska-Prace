namespace teacherpractise.model
{
    public class Practice
    {
        private int Id { get; set; }
        public UserPractice UserPractice { get; set; }

        private DateTime date { get; set; }
        private DateTime start { get; set; }
        private DateTime end { get; set; }
        private string? note { get; set; }
        private int capacity { get; set; }

        //subjectID is fake key to subject.Id
        private int subjectId { get; set; }
        public Subject Subject { get; set; }

        private int teacherId { get; set; }
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }
    }
}