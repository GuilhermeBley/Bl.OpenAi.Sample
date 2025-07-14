using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["ModelName"] ?? string.Empty;
string key = config["OpenAIKey"] ?? string.Empty;
string url = config["Url"] ?? string.Empty;

// Create the IChatClient
var client = new AzureOpenAIClient(
    new Uri(url), new ApiKeyCredential(key))
    .GetChatClient(model);

var prompt = ConstructPromptWithContext();

var chatCompletionsOptions = new ChatCompletionOptions()
{
    Temperature = 0.7f,
    MaxOutputTokenCount = 1000,
};

var response = await client.CompleteChatAsync(
    [
        new SystemChatMessage("Você é um especialista em veículos e legislação de infrações e débitos de veículos no Brasil. " +
            "Tente dar dicas e resolver problemas para seus clientes."),
        new UserChatMessage(prompt)
    ],
    chatCompletionsOptions);

Console.WriteLine("Resposta do modelo:");
Console.WriteLine(string.Join("\n",
    response.Value.Content.Select(x => x.Text))
);
Console.ReadKey();

string ConstructPromptWithContext()
{
    var fakeVehicleObj = new
    {
        BrlDateNow = DateTime.UtcNow.AddHours(-3),
        Multas = new object[] {
            new 
            {
                Tipo = "Excesso de velocidade",
                Valor = 150.00,
                Data = DateTime.UtcNow.AddDays(-10),
                DueDate = DateTime.UtcNow.AddDays(5),
            },
            new
            {
                Tipo = "Estacionamento proibido",
                Valor = 80.00,
                Data = DateTime.UtcNow.AddDays(-20),
                DueDate = DateTime.UtcNow.AddDays(10),
            },
            new
            {
                Tipo = "Uso de celular ao dirigir",
                Valor = 200.00,
                Data = DateTime.UtcNow.AddDays(-15),
                DueDate = DateTime.UtcNow.AddDays(2),
            },
        },
        LastLicenciamento = 2022,
        IpvaDebit = 20000,
        Vehicle = new
        {
            Placa = "ABC1234",
            Renavam = "12345678901",
            Chassi = "9BWZZZ377VT004251",
            Marca = "Volkswagen",
            Modelo = "Fusca",
            AnoFabricacao = 1976,
            AnoModelo = 1976,
            Cor = "Amarelo",
            TipoCombustivel = "Gasolina",
            Quilometragem = 150000,
            Uf = "SP",
        },
        IpvaDueDate = DateTime.UtcNow.AddDays(-3),
        LicenciamentoDueDate = DateTime.UtcNow.AddDays(-3),
    };

    var vehicleData = JsonSerializer.Serialize(fakeVehicleObj) ?? string.Empty;

    return 
        $"""
        {vehicleData}
        Com base no JSON anterior, responda as seguintes perguntas sobre o veículo abaixo:
        1. Quais são as características do veículo?
        2. Quais são as multas pendentes e seus detalhes?
        3. Qual é o status do licenciamento e do IPVA?
        4. Quais são as datas de vencimento das multas, licenciamento e IPVA?
        5. Quais são as ações recomendadas para regularizar a situação do veículo?
        """;
}