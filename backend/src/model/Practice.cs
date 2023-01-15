namespace teacherpractise.model
{
    public class Practice
    {
        private int Id { get; set; }
        private DateTime date { get; set; }
        private DateTime start { get; set; }
        private DateTime end { get; set; }
        private string note { get; set; }
        private int capacity { get; set; }
        private int subject_id { get; set; }
        private int teacher_id { get; set; }
    }
}