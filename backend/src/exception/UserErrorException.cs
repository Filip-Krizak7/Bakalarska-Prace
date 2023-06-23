using System;

namespace TeacherPractise.Exceptions
{
    [Serializable]
    public class UserErrorException : Exception
    {
        private string field = null;

        public UserErrorException() { }

        public UserErrorException(string message) : base(message) { }

        public UserErrorException(string message, string field) : base(message)
        {
            this.field = field;
        }

        public string Field
        {
            get { return field; }
        }
    }
}