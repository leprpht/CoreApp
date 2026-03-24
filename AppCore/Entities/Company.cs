namespace AppCore.Entities;

public class Company : Contact
{
    public string Name { get; set; } = string.Empty;
    public string? Nip { get; set; }
    public string? Region { get; set; }
    public string? Krs { get; set; }
    public string? Industry { get; set; }
    public int? EmployeeCount { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public string? Website { get; set; }
    public List<Person> Employees { get; set; } = [];
    public Person? PrimaryContact { get; set; }
}