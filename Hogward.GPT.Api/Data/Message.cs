namespace Hogward.GPT.Api.Data;

public class Message
{
    public DateTime TimeStamp { get; set; }
    public string? Body { get; set; }
    public bool IsRequest { get; set; }
}