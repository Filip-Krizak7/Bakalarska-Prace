using System.IO;
using System.Linq;
using TeacherPractise.Service;
using TeacherPractise.Model;
using TeacherPractise.Service.FileManagement;

namespace TeacherPractise.Service.FileService
{
    public class FileService
    {
        public bool deleteFile(string email, string fileName)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == email);
                string filePath = Path.Combine(FileUtil.folderPath, teacher.Id.ToString(), fileName);
                File.Delete(filePath);
                return true;
            }
        }

        public string figureOutFileNameFor(string teacherMail, string fileName)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherMail);
                return Path.Combine(FileUtil.folderPath, teacher.Id.ToString(), fileName);
            }
        }

        public string figureOutReportNameFor(long id)
        {
            try
            {
                string[] files = Directory.GetFiles(Path.Combine(FileUtil.reportsFolderPath, id.ToString()));
                string fileName = files.FirstOrDefault();
                return fileName;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}