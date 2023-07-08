using System.IO;
using System.Linq;
using TeacherPractise.Config;

namespace TeacherPractise.Service.FileManagement
{
    public static class FileUtil
    {
        public static readonly string folderPath = AppConfig.FOLDER_PATH;
        public static readonly string reportsFolderPath = AppConfig.REPORTS_FOLDER_PATH;
        public static readonly string filePath = Path.GetFullPath(folderPath);

        public static long getNumberOfFilesInFolder(long id)
        {
            try
            {
                var dir = new DirectoryInfo(Path.Combine(folderPath, id.ToString()));
                return dir.GetFiles().Length;
            }
            catch
            {
                return 999;
            }
        }

        public static long getNumberOfReportsInFolder(long id)
        {
            try
            {
                var dir = new DirectoryInfo(Path.Combine(reportsFolderPath, id.ToString()));
                return dir.GetFiles().Length;
            }
            catch
            {
                return 999;
            }
        }
    }
}