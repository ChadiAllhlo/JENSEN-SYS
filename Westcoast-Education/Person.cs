namespace Westcoast_Education;

class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? PersonalNumber { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Credentials { get; set; }
    public string? Responsibilities { get; set; }
    public DateTime EmploymentDate { get; set; }


    public override string ToString()
    {
        return $"Namn: {FirstName} {LastName} \nPersonnummer: {PersonalNumber} \nMobilnummer: {Phone} \nAddress: {Address} {PostalCode} {City}" ;
    }

}