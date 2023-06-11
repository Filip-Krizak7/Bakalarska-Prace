namespace TeacherPractise.Controller.FileManagement
{
    public class FileUploadResponse
    {
        public string message { get; set; }

        public FileUploadResponse(string message)
        {
            this.message = message;
        }
    }
}