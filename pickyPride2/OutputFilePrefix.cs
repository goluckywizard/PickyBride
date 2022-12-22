namespace pickyPride2;

public class OutputFilePrefix
{
    public string FilePrefix { get; set; } = String.Empty;

    public OutputFilePrefix(string filePrefix)
    {
        FilePrefix = filePrefix;
    }

    public OutputFilePrefix()
    {
    }
}