namespace DifyAi.InternalException;

public class DifyConfigMissingException : Exception
{
    public DifyConfigMissingException()
    {
    }

    public DifyConfigMissingException(string message) : base(message)
    {
    }

    public DifyConfigMissingException(string message, Exception inner) : base(message, inner)
    {
    }
}