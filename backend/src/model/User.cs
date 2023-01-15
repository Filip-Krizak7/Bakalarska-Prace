namespace teacherpractise.model
{
    public class User
    {
        private int Id { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private string firstName { get; set; }
        private string secondName { get; set; }
        private string phoneNumber { get; set; }
        private int role_id { get; set; }
        private bool locked { get; set; }
        private bool enabled { get; set; }
        private int school_id { get; set; }
        private int teacher_practice_id { get; set; }
        private int student_practice_id { get; set; }
    }
}