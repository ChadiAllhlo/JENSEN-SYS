namespace Westcoast_Education;

class AddCourses
{

    public Courses? Courses { get; set; }
    public List<Courses> CourseList { get; set; } = [];

    public AddCourses()
    {
        CourseList = new List<Courses>();
        var Course = new Courses();
        Course.CourseId = 113;
        Course.Name = "Religion 2B";
        Course.Length = 140;
        Course.StartDate = new DateTime(2024, 2, 12);
        Course.EndDate = Course.StartDate.AddDays(Course.Length);
        CourseList.Add(Course);

        Course = new Courses();
        Course.CourseId = 114;
        Course.Name = "Religion 3B";
        Course.Length = 140;
        Course.StartDate = new DateTime(2025, 2, 12);
        Course.EndDate = Course.StartDate.AddDays(Course.Length);
        CourseList.Add(Course);

        Course = new Courses();
        Course.CourseId = 223;
        Course.Name = "Matematik 2B";
        Course.Length = 140;
        Course.StartDate = new DateTime(2024, 2, 12);
        Course.EndDate = Course.StartDate.AddDays(Course.Length);
        CourseList.Add(Course);

        Course = new Courses();
        Course.CourseId = 224;
        Course.Name = "Matematik 3B";
        Course.Length = 140;
        Course.StartDate = new DateTime(2025, 2, 12);
        Course.EndDate = Course.StartDate.AddDays(Course.Length);
        CourseList.Add(Course);
        
    }
    public void PrintCourseList()
    {

        Console.WriteLine("Listar alla kurser\n");

        foreach (var Course in CourseList)
        {
            Console.WriteLine($"{Course}");
            Console.WriteLine("--------------------------------");
        }
    }
}