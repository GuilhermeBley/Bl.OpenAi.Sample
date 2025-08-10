
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


var chatCompletionsOptions = new ChatCompletionOptions()
{
    MaxOutputTokenCount = 20_000
};
#pragma warning disable AOAI001
chatCompletionsOptions.SetNewMaxCompletionTokensPropertyEnabled(true);
#pragma warning restore AOAI001


do
{
    Console.WriteLine("Enter your prompt (or type 'exit' to quit):");
    var prompt = Console.ReadLine() ?? string.Empty;

    if (prompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting the chat...");
        Console.ReadKey();
        return;
    }

     var response = await client.CompleteChatAsync(
        [
            new SystemChatMessage("Limit answer in 19000 characters."),
        new SystemChatMessage("You're an dotnet specialist, help people to understand codes."),
        new SystemChatMessage("If the subject is not about dotnet or microsoft related tools, please type: 'Please, let talk about .NET and its tecnologies.'."),
        new UserChatMessage(prompt)
        ],
        chatCompletionsOptions);

    Console.WriteLine("Response:");
    Console.WriteLine(string.Join("\n", response.Value.Content.Select(x => x.Text)));
} while (true);