namespace TeacherPractise.Dto.Response
{
    public class ReviewDto {
        public long practiceId { get; set; }
        public string name { get; set; }
        public string reviewText { get; set; }

        public ReviewDto(long practiceId, string name, string reviewText)
        {
            this.practiceId = practiceId;
            this.name = name;
            this.reviewText = reviewText;
        }

        public ReviewDto()
        {}
    }
}