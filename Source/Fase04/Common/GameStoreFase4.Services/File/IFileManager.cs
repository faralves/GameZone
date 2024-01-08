namespace GameStoreFase4.Services.File;
public interface IFileManager
{
    void Save(string message, bool salvoEmBD = false);
    void Save(List<string> message, bool salvoEmBD = false);

    List<string> GetMessages();
    void CleanDlqFile();
}
