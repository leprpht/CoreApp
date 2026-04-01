using AppCore.Entities;

namespace AppCore.Dto;

public record NoteDto(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    string CreatedBy)
{
    public static NoteDto FromEntity(Note note) =>
        new(note.Id, note.Content, note.CreatedAt, note.CreatedBy);
}

public record CreateNoteDto(string Content, string? CreatedBy = null);