namespace AppCore.Entities;

public class Position : EntityBase
{
    public string Title { get; set; } = string.Empty; // English job title
    public string? Description { get; set; }
}