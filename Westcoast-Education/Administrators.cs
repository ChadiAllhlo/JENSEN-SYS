namespace Westcoast_Education;

class Administrators
{
    public Person? Person { get; set; }
    public List<Person> AdministratorsList { get; set; }

    public Administrators()
    {
        AdministratorsList = new List<Person>();

        var Administrator = new Person();
        Administrator.FirstName = "Hasse";
        Administrator.LastName = "Svensson";
        Administrator.Phone = "0402153255";
        Administrator.PersonalNumber = "198505127542";
        Administrator.Address = "Balladgatan 32";
        Administrator.PostalCode = "27425";
        Administrator.City = "Malmö";
        Administrator.Credentials = "Kristianstad Universitet, Administratör";
        Administrator.Responsibilities = "Ansvarar för alla klasser med start 2022";
        Administrator.EmploymentDate = new DateTime(2020,05,20);
        AdministratorsList.Add(Administrator);

        Administrator = new Person();
        Administrator.FirstName = "Hassan";
        Administrator.LastName = "Mohammed";
        Administrator.Phone = "0402653225";
        Administrator.PersonalNumber = "197505129563";
        Administrator.Address = "Tessins väg 15";
        Administrator.PostalCode = "22465";
        Administrator.City = "Malmö";
        Administrator.Credentials = "Yrkeshögskola, Administratör";
        Administrator.Responsibilities = "Ansvarar för alla klasser med start 2023";
        Administrator.EmploymentDate = new DateTime(2022,08,05);
        AdministratorsList.Add(Administrator);
    }

    public void PrintAdministratorsList(){
        
        Console.WriteLine("Listar alla Utbildningsledare\n");
        
        foreach(var Administrator in AdministratorsList){
            Console.WriteLine($"{Administrator}\nKunskapsområden: {Administrator.Credentials}\nAnsvarsområden: {Administrator.Responsibilities}\nAnställningsdatum: {Administrator.EmploymentDate:yyyy-MM-dd}");
            Console.WriteLine("--------------------------------");
        }
    }
}