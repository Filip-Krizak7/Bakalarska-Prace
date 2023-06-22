using System.Net;
using TeacherPractise.Model;
using TeacherPractise.Config;
using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using TeacherPractise.Service.FileManagement;
using TeacherPractise.Dto.Response;
using TeacherPractise.Service.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TeacherPractise.Controller.FileManagement
{
    [Route("teacher")]
    [Authorize]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly FileService fileService;
        private readonly AppUserService appUserService;

        public UploadFileController(
            [FromServices] FileService fileService, 
            [FromServices] AppUserService appUserService)
        {
            this.fileService = fileService;
            this.appUserService = appUserService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            try
            {
                using (var ctx = new Context())
                {
                    string currentEmail = appUserService.getCurrentUserEmail();
                    var id = ctx.Users.ToList().FirstOrDefault(q => q.Username == currentEmail).Id;
                    var userFolderPath = new DirectoryInfo(Path.Combine(FileUtil.folderPath, id.ToString()));
                    createDirIfNotExist(userFolderPath);
                    
                    var maxFiles = AppConfig.MAXIMUM_FILE_NUMBER_PER_USER;
                    var numberOfFilesUploaded = files.Count;
                    
                    var filesNum = FileUtil.getNumberOfFilesInFolder(id);
                    if ((filesNum + numberOfFilesUploaded) > maxFiles)
                    {
                        return StatusCode((int)HttpStatusCode.ExpectationFailed,
                            new FileUploadResponse("Byl překročen limit počtu souborů na uživatele. Maximum je: " + maxFiles));
                    }

                    var fileNames = new List<string>();

                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(userFolderPath.FullName, fileName);
                        
                        if (fileExists(id, fileName))
                        {
                            fileName = renameExistingFile(userFolderPath, fileName);
                            filePath = Path.Combine(userFolderPath.FullName, fileName);
                        }
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        fileNames.Add(fileName);
                    }

                    return Ok(new FileUploadResponse("Soubory byly úspěšně nahrány: " + string.Join(", ", fileNames)));
                }
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.ExpectationFailed,
                    new FileUploadResponse("Došlo k neočekávané chybě!"));
            }
        }

        private bool fileExists(long id, string fileName)
        {
            var path = Path.Combine(FileUtil.folderPath, id.ToString(), fileName);
            return System.IO.File.Exists(path);
        }

        [HttpPost("report/upload")]
        public async Task<IActionResult> uploadReport([FromForm] long id, [FromForm] List<IFormFile> file)
        {
            try
            {
                var userFolderPath = new DirectoryInfo(FileUtil.reportsFolderPath + id);
                var userFilePath = new DirectoryInfo(Path.Combine(FileUtil.folderPath, id.ToString()));
                createDirIfNotExist(userFolderPath);
                var maxFiles = AppConfig.MAXIMUM_NUMBER_OF_REPORTS;
                var numberOfFilesUploaded = file.Count;

                var filesNum = FileUtil.getNumberOfReportsInFolder(id);
                if ((filesNum + numberOfFilesUploaded) > maxFiles)
                {
                    var name = fileService.figureOutReportNameFor(id);

                    var fileReport = new FileInfo(name);

                    if (fileReport.Exists)
                    {
                        fileReport.Delete();
                        Console.WriteLine("File deleted successfully");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete the file");
                    }
                }

                var fileNames = new List<string>();

                foreach (var f in file)
                {
                    var fileName = Path.GetFileName(f.FileName);
                    var filePath = Path.Combine(userFolderPath.FullName, fileName);

                    var path = new FileInfo(Path.Combine(FileUtil.folderPath, id.ToString(), fileName));
                    
                    if (fileExists(id, fileName))
                    {
                        fileName = renameExistingFile(userFilePath, fileName);
                        filePath = Path.Combine(userFolderPath.FullName, fileName);
                    }
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await f.CopyToAsync(stream);
                    }

                    fileNames.Add(fileName);
                }

                return Ok(new FileUploadResponse("Soubory byly úspěšně nahrány: " + string.Join(", ", fileNames)));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.ExpectationFailed,
                    new FileUploadResponse("Došlo k neočekávané chybě!"));
            }
        }

        private string renameExistingFile(DirectoryInfo path, string fileName)
        {
            int highestNumber = 1;
            DirectoryInfo directory = path.Parent;
            FileInfo[] matchingFiles = directory.GetFiles().Where(file => file.Name.StartsWith(fileName.Split('.')[0])).ToArray();
            Regex myPattern = new Regex(@"(\d+)");
            foreach (var file in matchingFiles)
            {
                var m = myPattern.Match(file.Name);
                while (m.Success)
                {
                    string s = m.Groups[m.Groups.Count - 1].Value;
                    if (int.Parse(s) > highestNumber)
                        highestNumber = int.Parse(s);
                    m = m.NextMatch();
                }
            }
            string name = fileName.Split('.')[0];
            string fileExtension = fileName.Split('.')[1];
            string ret = $"{name}({highestNumber + 1}).{fileExtension}";
            return ret;
        }

        private void createDirIfNotExist(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
                if (!directory.Exists)
                {
                    Console.WriteLine("was not successful.");
                }
            }
        }

        [HttpGet("files")]
        public IActionResult getListFiles()
        {
            Console.WriteLine("Called files list endpoint /teacher/files");
            string[] files = Directory.GetFiles(FileUtil.folderPath).Select(Path.GetFileName).ToArray();
            return Ok(files);
        }
    }
}