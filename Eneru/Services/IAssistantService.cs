namespace Eneru.Services
{
    public interface IAssistantService
    {
        // Takes user message, returns Gemini response
        Task<string> AskAsync(string userMessage);
    }
}