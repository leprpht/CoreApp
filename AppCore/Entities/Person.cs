namespace AppCore.Entities;

public class Person : Contact
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;

    public string FullName => MiddleName == null
        ? $"{FirstName} {LastName}"
        : $"{FirstName} {MiddleName} {LastName}";

    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Position { get; set; }
    public Company? Employer { get; set; }
}