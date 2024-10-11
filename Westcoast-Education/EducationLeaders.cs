namespace Westcoast_Education;

class EducationLeaders
{
    public Person? Person { get; set; }
    public List<Person> EducationalLeadersList { get; set; }

    public EducationLeaders()
    {
        EducationalLeadersList = new List<Person>();

        var EducationalLeader = new Person();
        EducationalLeader.FirstName = "Tomas";
        EducationalLeader.LastName = "Lindahl";
        EducationalLeader.Phone = "0760012337";
        EducationalLeader.PersonalNumber = "197311055368";
        EducationalLeader.Address = "Roslins väg 1";
        EducationalLeader.PostalCode = "21755";
        EducationalLeader.City = "Malmö";
        EducationalLeader.Credentials = "Malmö Universitet, Utbildningsledare";
        EducationalLeader.Responsibilities = "Ansvarar för Klasserna EK23B / NA23A / EK24B / NA24A";
        EducationalLeader.EmploymentDate = DateTime.Now;
        EducationalLeadersList.Add(EducationalLeader);

        EducationalLeader = new Person();
        EducationalLeader.FirstName = "Kristina";
        EducationalLeader.LastName = "Olsson";
        EducationalLeader.Phone = "0769512356";
        EducationalLeader.PersonalNumber = "196812095483";
        EducationalLeader.Address = "Vitemöllegatan 7";
        EducationalLeader.PostalCode = "21525";
        EducationalLeader.City = "Malmö";
        EducationalLeader.Credentials = "Linköpings Universitet, Utbildningsledare";
        EducationalLeader.Responsibilities = "Ansvarar för Klasserna SA23A / SA23B / SA24A / SA24B";
        EducationalLeader.EmploymentDate = DateTime.Now;
        EducationalLeadersList.Add(EducationalLeader);
    }
    public void PrintEducationalLeadersList(){
        
        Console.WriteLine("Listar alla Utbildningsledare\n");
        
        foreach(var EducationalLeader in EducationalLeadersList){
            Console.WriteLine($"{EducationalLeader}\nKunskapsområden: {EducationalLeader.Credentials}\nAnsvarsområden: {EducationalLeader.Responsibilities}\nAnställningsdatum: {EducationalLeader.EmploymentDate:yyyy-MM-dd}");
            Console.WriteLine("--------------------------------");
        }
    }
}