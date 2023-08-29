using System.Text;
using CsvHelper;
using TeacherPractise.Model;

namespace TeacherPractise.Service.CsvReport
{
    public class CsvReport
    {
        public string createReport(string filePath, DateTime start, DateTime end)
        {
            using (var ctx = new Context())
	        {
                //List<Practice> practices = practiceRepository.FindByDateBetweenAndDateLessThanEqual(start, end, DateTime.Now.Date);
                List<Practice> practices = ctx.Practices.Where(p => p.Date >= start && p.Date <= end && p.Date <= DateTime.Now).ToList();

                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.WriteField("Předmět");
                    csv.WriteField("Datum");
                    csv.WriteField("Od");
                    csv.WriteField("Do");
                    csv.WriteField("Škola");
                    csv.WriteField("Učitel");
                    csv.WriteField("Studenti");
                    csv.NextRecord();

                    foreach (var p in practices)
                    {
                        var students = p.UsersOnPractice;
                        User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Id == p.TeacherId)
                	        ?? throw AppUserService.CreateException($"Učitel s ID {p.TeacherId} neexistuje.");
                        School school = ctx.Schools.ToList().FirstOrDefault(q => q.Id == teacher.SchoolId)
                	        ?? throw AppUserService.CreateException($"Škola s ID {teacher.SchoolId} neexistuje.");
                        Subject subject = ctx.Subjects.ToList().FirstOrDefault(q => q.Id == p.SubjectId)
                	        ?? throw AppUserService.CreateException($"Předmět s ID {p.SubjectId} neexistuje.");

                        csv.WriteField(subject.Name);
                        csv.WriteField(p.Date.ToString());
                        csv.WriteField(p.Start.ToString(@"hh\:mm"));
                        csv.WriteField(p.End.ToString(@"hh\:mm"));
                        /*csv.WriteField(p.Start.Hour + ":" + (p.Start.Minute == 0 ? "00" : p.Start.Minute.ToString()));
                        csv.WriteField(p.End.Hour + ":" + (p.End.Minute == 0 ? "00" : p.End.Minute.ToString()));*/
                        csv.WriteField(school.Name);
                        csv.WriteField(teacher.FirstName + " " + teacher.SecondName + " (" + teacher.Username + ")");
                        csv.WriteField(students.Count > 0 ? students.First().FirstName + " " + students.First().SecondName + " (" + students.First().Username + ")" : "---");
                        csv.NextRecord();

                        if (students.Count > 0)
                        {
                            students.Remove(students.First());
                            foreach (var s in students)
                            {
                                csv.WriteField("");
                                csv.WriteField("");
                                csv.WriteField("");
                                csv.WriteField("");
                                csv.WriteField("");
                                csv.WriteField("");
                                csv.WriteField(s.FirstName + " " + s.SecondName + " (" + s.Username + ")");
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }

            return "export success";
        }
    }
}