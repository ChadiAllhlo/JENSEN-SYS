namespace Westcoast_Education;

class Courses
{
    public int CourseId { get; set; }
    public string? Name { get; set; }
    public int Length { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public override string ToString()
    {
        return $"Kursnummer: {CourseId}\nKursnamn: {Name}\nKurslängd: {Length} dagar\nStartdatum: {StartDate:yyyy-MM-dd}\nSlutdatum: {EndDate:yyyy-MM-dd}";
    }
}