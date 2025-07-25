using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;

namespace Bl.OpenAi.Sample;

public class VehicleTipsClientChat
{
    private readonly ChatClient _client;

    public VehicleTipsClientChat(string key, string url, string model)
    {
        var clientOpenAi = new AzureOpenAIClient(
            new Uri(url),
            new AzureKeyCredential(key));

        _client = clientOpenAi.GetChatClient(model);
    }
}
