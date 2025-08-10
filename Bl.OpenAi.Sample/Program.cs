using Bl.OpenAi.Sample;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["ModelName"] ?? string.Empty;
string key = config["OpenAIKey"] ?? string.Empty;
string url = config["Url"] ?? string.Empty;

var vehicleTipsClientChat = new VehicleTipsClientChat(key: key, url: url, model: model);

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

var response = await vehicleTipsClientChat.GeneratePromptAsync(fakeVehicleObj);

Console.WriteLine("Resposta do modelo:");
Console.WriteLine(string.Join("\n",
    response.Content.Select(x => x.Text))
);
Console.ReadKey();