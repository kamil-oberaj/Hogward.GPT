using Hogward.GPT.Api.Data;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Hogward.GPT.Api.Services;

public sealed class ChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly ChatHistory _chatHistory;
    private readonly Kernel _kernel;

    public ChatService(ChatHistory chatHistory, Kernel kernel, ILogger<ChatService> logger)
    {
        _chatHistory = chatHistory;
        _kernel = kernel;
        _logger = logger;
    }
    
    public async Task<Message> GetResponseAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {

            var service = _kernel.GetRequiredService<IChatCompletionService>();
            _chatHistory.AddUserMessage(message);

            var response = await service.GetChatMessageContentAsync(_chatHistory,
                kernel: _kernel,
                cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(response.Content))
            {
                _logger.LogError($"Something went wrong. Response is empty. {System.Text.Json.JsonSerializer.Serialize(response)}");
                throw new ApplicationException($"Something went wrong. Response is empty. {System.Text.Json.JsonSerializer.Serialize(response)}");
            }
            
            _chatHistory.AddAssistantMessage(response.Content);

            return new Message
            {
                Body = response.Content,
                IsRequest = false,
                TimeStamp = DateTime.UtcNow
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Well something not worked. {Environment.NewLine} Exception: {e.Message}");
            return new Message
            {
                Body = $"Well something not worked. {Environment.NewLine} Exception: {e.Message}",
                IsRequest = false,
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}