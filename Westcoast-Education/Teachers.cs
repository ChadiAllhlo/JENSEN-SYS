namespace Westcoast_Education;

class Teachers
{
    public Person? Person { get; set; }
    public List<Person> TeacherList { get; set; }

    public Teachers()
    {
        TeacherList = new List<Person>();

        var Teacher = new Person();
        Teacher.FirstName = "Anna";
        Teacher.LastName = "Lindqvist";
        Teacher.Phone = "0703512315";
        Teacher.PersonalNumber = "196810053350";
        Teacher.Address = "branteviksgatan 2";
        Teacher.PostalCode = "15575";
        Teacher.City = "Malmö";
        Teacher.Credentials = "Malmö Universitet, Matematik och Naturkunskap";
        Teacher.Responsibilities = "Ansvarar för Klasserna EK24B & NA24A";
        TeacherList.Add(Teacher);

        Teacher = new Person();
        Teacher.FirstName = "Kristina";
        Teacher.LastName = "Olsson";
        Teacher.Phone = "0769512356";
        Teacher.PersonalNumber = "196812095483";
        Teacher.Address = "Vitemöllegatan 7";
        Teacher.PostalCode = "21525";
        Teacher.City = "Malmö";
        Teacher.Credentials = "Malmö Universitet, Idrott och Religion";
        Teacher.Responsibilities = "Ansvarar för Klasserna EK23B & NA23A";
        TeacherList.Add(Teacher);
    }
    public void PrintTeacherList()
    {

        Console.WriteLine("Listar alla lärare\n");

        foreach (var Teacher in TeacherList)
        {
            Console.WriteLine($"{Teacher}\nKunskapsområden: {Teacher.Credentials}\nAnsvarsområden: {Teacher.Responsibilities}");
            Console.WriteLine("--------------------------------");
        }
    }
}