using Microsoft.Extensions.Options;
using PickyBride.Data.Entities;

namespace pickyPride2;

public class FileResultSaver : IResultSaver
{
    private Contender[]? _contenders;
    public Contender[]? Contenders
    {
        private get
        {
            return _contenders;
        }
        set
        {
            _contenders = value;
        }
    }

    private IOptions<OutputFilePrefix>? _optionsFilePrefix;

    public FileResultSaver()
    {
        _optionsFilePrefix = null;
    }

    public FileResultSaver(IOptions<OutputFilePrefix> optionsFilePrefix)
    {
        try
        {
            _optionsFilePrefix = optionsFilePrefix;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private int CheckResult(Contender? result)
    {
        if (result is null)
            return 10;
        if (result.Rating > 50)
            return result.Rating;
        else
            return 0;
    }
    
    public void SaveResult(Contender? res)
    {
        if (_contenders is null)
        {
            throw new ArgumentNullException();
        }

        string prefix = _optionsFilePrefix?.Value.FilePrefix ?? "prefix";
        string path = prefix + DateTime.Now.ToString("log hh.mm.ss") + "-" + ".txt";
        
        List<string> result = Array.ConvertAll(Contenders, input => input.Name + ": " + input.Rating).ToList();
        result.Add(new String('-', 30));
        result.Add("" + CheckResult(res));
        File.WriteAllLines(path, result);
    }
}