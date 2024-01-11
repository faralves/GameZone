using Microsoft.Extensions.Configuration;
using System.Text;

namespace GameStoreFase4.Services.File;
public class FileManagerService : IFileManagerService
{
    private readonly IConfiguration _configuration;

    private static string _filePath = null;
    private static string _filename = null;
    private static string _filenameDB = null;

    public FileManagerService(IConfiguration configuration)
    {
        _configuration = configuration;
        _filePath = configuration["FileManager:DlqMessageConfig:FilePath"];
        _filename = configuration["FileManager:DlqMessageConfig:Filename"];
        _filenameDB = configuration["FileManager:DlqMessageConfig:FilenameDB"];
    }
    public void Save(string message, bool salvoEmBD = false)
    {
        if (!Directory.Exists(_filePath))
            Directory.CreateDirectory(_filePath);

        string path = $"{_filePath}{_filename}";
        if (salvoEmBD)
            path = $"{_filePath}{_filenameDB}";

        StreamWriter file = new StreamWriter(path, true, Encoding.UTF8);
        file.WriteLine(message);
        file.Close();
    }
    public void Save(List<string> messages, bool salvoEmBD = false)
    {
        if (!Directory.Exists(_filePath))
            Directory.CreateDirectory(_filePath);

        string path = $"{_filePath}{_filename}";
        if (salvoEmBD)
            path = $"{_filePath}{_filenameDB}";

        StreamWriter file = new StreamWriter(path, true, Encoding.UTF8);
        foreach (var message in messages)
            file.WriteLine(message);

        file.Close();
    }

    public List<string> GetMessages()
    {
        List<string> result = new List<string>();
        string path = $"{_filePath}{_filename}";
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
            return null;

        StreamReader file = new StreamReader(path, Encoding.UTF8);
        string content = file.ReadToEnd();
        file.Close();

        var linhas = content.Split(Char.Parse("\n"));
        result.AddRange(linhas);
        return result;
    }
    public void CleanDlqFile()
    {
        string path = $"{_filePath}{_filename}";
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Delete();
    }
}
