using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace Westcoast_Education;

class StudentList
{
    public Person? Person { get; set; }
    public List<Person> StudentLists { get; set; }

    public StudentList()
    {

        StudentLists = new List<Person>();
        var Student = new Person();

        Student.FirstName = "Calle";
        Student.LastName = "Karlsson";
        Student.Phone = "0765233215";
        Student.PersonalNumber = "199201055524";
        Student.Address = "Limhamnsvägen 12";
        Student.PostalCode = "21575";
        Student.City = "Malmö";
        StudentLists.Add(Student);

        Student = new Person();
        Student.FirstName = "Jonas";
        Student.LastName = "Svensson";
        Student.Phone = "07302330040";
        Student.PersonalNumber = "198211101577";
        Student.Address = "Kalkvägen 22";
        Student.PostalCode = "21265";
        Student.City = "Malmö";
        StudentLists.Add(Student);

    }
    public void PrintStudentList()
    {

        Console.WriteLine("Listar alla studenter\n");

        foreach (var Student in StudentLists)
        {
            Console.WriteLine($"{Student}");
            Console.WriteLine("--------------------------------");
        }
    }
}