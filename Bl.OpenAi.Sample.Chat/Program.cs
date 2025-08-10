using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["ModelName"] ?? string.Empty;
string key = config["OpenAIKey"] ?? string.Empty;
string url = config["Url"] ?? string.Empty;

if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(url))
{
    Console.WriteLine("Please set the ModelName, OpenAIKey, and Url in your user secrets.");
    Console.ReadKey();
    return;
}

var clientOpenAi = new AzureOpenAIClient(
    new Uri(url),
    new AzureKeyCredential(key));

var client = clientOpenAi.GetChatClient(model);

// Initialize conversation history with system messages
var conversationHistory = new List<ChatMessage>
{
    new SystemChatMessage("Limit answer in 19000 characters."),
    new SystemChatMessage("You're a dotnet specialist, help people to understand codes."),
    new SystemChatMessage("If the subject is not about dotnet or microsoft related tools, please type: 'Please, lets talk about .NET and its technologies.'.")
};

var chatCompletionsOptions = new ChatCompletionOptions()
{
    MaxOutputTokenCount = 20_000,
};
#pragma warning disable AOAI001
chatCompletionsOptions.SetNewMaxCompletionTokensPropertyEnabled(true);
#pragma warning restore AOAI001

Console.WriteLine("Chat started. Type 'exit' to quit or 'clear' to reset the conversation.");
do
{
    Console.Write("> Enter your prompt: ");
    var prompt = Console.ReadLine() ?? string.Empty;

    if (prompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting the chat...");
        Console.ReadKey();
        return;
    }

    if (prompt.Equals("clear", StringComparison.OrdinalIgnoreCase))
    {
        // Reset conversation history while keeping system messages
        conversationHistory.RemoveAll(m => m is UserChatMessage || m is AssistantChatMessage);
        Console.WriteLine("Conversation history cleared.");
        continue;
    }

    // Add user message to history
    conversationHistory.Add(new UserChatMessage(prompt));

    var response = await client.CompleteChatAsync(conversationHistory, chatCompletionsOptions);

    // Add assistant response to history
    var assistantResponse = response.Value.Content[0].Text;
    conversationHistory.Add(new AssistantChatMessage(assistantResponse));

    Console.WriteLine("Response:");
    Console.WriteLine(assistantResponse);
} while (true);