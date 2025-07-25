using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using System.Text.Json;
using Azure.AI.OpenAI.Chat;
using System.Threading.Tasks;

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

    public async Task<ChatCompletion> GeneratePromptAsync(object? vehicleData)
    {
        var prompt = ConstructPromptWithContext(vehicleData ?? new { });

        var chatCompletionsOptions = new ChatCompletionOptions()
        {
            MaxOutputTokenCount = 20_000
        };
#pragma warning disable AOAI001
        chatCompletionsOptions.SetNewMaxCompletionTokensPropertyEnabled(true);
#pragma warning restore AOAI001

        var response = await _client.CompleteChatAsync(
            [
                new SystemChatMessage("Limite as resposta em no máximo 19000 caracteres."),
        new SystemChatMessage("Você é um especialista em veículos e legislação de infrações e débitos de veículos no Brasil. " +
            "Tente dar dicas e resolver problemas para seus clientes."),
        new UserChatMessage(prompt)
            ],
            chatCompletionsOptions);

        return response.Value;
    }

    private string ConstructPromptWithContext(object vehicleDataObj)
    {

        var vehicleData = JsonSerializer.Serialize(vehicleDataObj) ?? string.Empty;

        return
            $"""
        {vehicleData}
        Com base no JSON anterior, responda as seguintes perguntas sobre o veículo abaixo:
        1. Quais são as características do veículo?
        2. Quais são as multas pendentes e seus detalhes?
        3. Qual é o status do licenciamento e do IPVA?
        4. Quais são as datas de vencimento das multas, licenciamento e IPVA?
        5. Quais são as ações recomendadas para regularizar a situação do veículo?

        Se algum dos campos não for fornecido, não responda a pergunta.
        """;
    }
}
