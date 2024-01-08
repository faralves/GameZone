namespace GameStoreFase4.Domain.Entities;
public class ObjectMessageProcessedInfo
{
    public Jogo Object { get; set; }
    public bool ProcessedSuccessfully { get; set; }
    public bool ProcessedDlqQueue { get; set; }
}
