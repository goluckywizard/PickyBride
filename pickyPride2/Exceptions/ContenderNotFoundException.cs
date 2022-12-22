namespace pickyPride2;

public class ContenderNotFoundException : Exception
{
    public ContenderNotFoundException()
    {
    }

    public ContenderNotFoundException(string? message) : base(message)
    {
    }

    public ContenderNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}