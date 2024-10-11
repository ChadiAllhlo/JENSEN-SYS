using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Westcoast_Education;

class Program
{
    static void Main()
    {
        try
        {
            int x = 0;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Välkommen till WestCoast Education!");
            Console.ResetColor();

            while (x == 0)
            {
                Console.WriteLine(" ");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Välj något av följande alternativ:");
                Console.WriteLine("Tryck A för att visa alla studenter");
                Console.WriteLine("Tryck B för att visa alla kurser");
                Console.WriteLine("Tryck C för att visa alla lärare");
                Console.WriteLine("Tryck D för att visa alla utbildningsansvariga");
                Console.WriteLine("Tryck E för att visa alla administratörer");

                Console.WriteLine("Tryck q för att avbryta");
                Console.WriteLine(" ");

                var Input = Console.ReadLine();

                if (Input == "A" || Input == "a")
                {

                    var studentList = new StudentList();
                    studentList.PrintStudentList();
                }
                else if (Input == "B" || Input == "b")
                {
                    var coursesList = new AddCourses();
                    // coursesList.PrintCourseList();

                    var options = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };

                    var path = string.Concat(Environment.CurrentDirectory, "/Data/courses.json");
                    var json = JsonSerializer.Serialize(coursesList.CourseList, options);
                    File.WriteAllText(path, json);

                    options = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var savedJson = File.ReadAllText(path);

                    var courses = JsonSerializer.Deserialize<List<Courses>>(savedJson, options);
                    foreach (var course in courses!)
                    {
                        Console.WriteLine($"CourseId: {course.CourseId}, Name: {course.Name}, Length: {course.Length}, StartDate: {course.StartDate:yyyy-MM-dd}, EndDate: {course.EndDate:yyyy-MM-dd}");
                    }

                }
                else if (Input == "C" || Input == "c")
                {
                    var teacherList = new Teachers();
                    teacherList.PrintTeacherList();
                }
                else if (Input == "D" || Input == "d")
                {
                    var educationalLeadersList = new EducationLeaders();
                    educationalLeadersList.PrintEducationalLeadersList();
                }
                else if (Input == "E" || Input == "e")
                {
                    var administratorList = new Administrators();
                    administratorList.PrintAdministratorsList();
                }
                else if (Input == "Q" || Input == "q")
                {
                    x = 1;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fel inmatning. Försök igen.");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {

            throw new ArgumentException("Ett fel upstod", ex);
        }
    }
}
