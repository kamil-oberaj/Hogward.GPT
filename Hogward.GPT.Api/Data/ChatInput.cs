namespace Hogward.GPT.Api.Data;

public class ChatInput
{
    public string Text { get; set; }

    public ChatInput()
    {
        Text = string.Empty;
    }
    
    public void Clear()
    {
        Text = string.Empty;
    }
}