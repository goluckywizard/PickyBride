using System.Runtime.Serialization;

namespace pickyPride2;

public class NotViewedContenderException : Exception
{
    public NotViewedContenderException()
    {
    }

    protected NotViewedContenderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NotViewedContenderException(string? message) : base(message)
    {
    }

    public NotViewedContenderException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}