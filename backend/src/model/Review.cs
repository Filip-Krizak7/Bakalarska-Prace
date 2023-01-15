namespace teacherpractise.model
{
    public class Review
    {
        private int Id { get; set; }

        private int userId { get; set; }
        public User User { get; set; }

        private int practiceId { get; set; }
        public Practice Practice { get; set; }

        private string? text { get; set; }
    }
}