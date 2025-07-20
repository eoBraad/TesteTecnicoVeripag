namespace Domain.Messages;

public class LogMessage
{
    public List<string> Messages { get; set; }
    
    public DateTime Date { get; set; } = DateTime.Now;
    
    public string Path { get; set; }
    
    public LogMessage(List<string> messages, string path)
    {
        Messages = messages;
        Path = path;
    }
}