namespace AppCore.Exceptions;

public class NoteNotFoundException : Exception
{
    public NoteNotFoundException(string msg) : base(msg)
    {
    }
}