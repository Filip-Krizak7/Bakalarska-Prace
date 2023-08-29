using TeacherPractise.Dto.Response;
using System.Globalization;

namespace TeacherPractise.Dto.Request
{
    public class NewPracticeDto {
        public string _dateString;
        private string _startTimeString;
        private string _endTimeString;

        public string dateString
        {
            get { return _dateString; }
            set
            {
                _dateString = value;
                date = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //date = CustomDateOnly.FromString(dateString);
            }
        }

        public string startTimeString
        {
            get { return _startTimeString; }
            set
            {
                _startTimeString = value;
                DateTime.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeValue);
                start = timeValue.TimeOfDay;
            }
        }

        public string endTimeString
        {
            get { return _endTimeString; }
            set
            {
                _endTimeString = value;
                DateTime.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeValue);
                end = timeValue.TimeOfDay;
            }
        }

        public DateTime date { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public string note { get; set; }
        public int capacity { get; set; }
        public SubjectDto subject { get; set; }
    }
}