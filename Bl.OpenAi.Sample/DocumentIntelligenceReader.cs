

using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace Bl.OpenAi.Sample;

public class DocumentIntelligenceReader
{
    private readonly DocumentAnalysisClient _client;

    public DocumentIntelligenceReader(Uri uri, AzureKeyCredential credential)
    {
        _client = new DocumentAnalysisClient(uri, credential);
    }

    public async Task<CrlvInfo?> ReadCrlvAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var operation = await _client.AnalyzeDocumentAsync(waitUntil: WaitUntil.Completed, "prebuilt-id-crlv", stream, cancellationToken: cancellationToken);
        var result = operation.Value;

        string placa = string.Empty;
        string renavam = string.Empty;
        string dataEmissao = string.Empty;
        string? observacao = string.Empty;
        foreach (var doc in result.Documents)
        {
            if (doc.Fields.TryGetValue("placa", out var placaField) && placaField.FieldType == DocumentFieldType.String)
            {
                placa = placaField.Content;
            }
            if (doc.Fields.TryGetValue("renavam", out var renavamField) && renavamField.FieldType == DocumentFieldType.String)
            {
                renavam = renavamField.Content;
            }
            if (doc.Fields.TryGetValue("data_emissao", out var dataEmissaoField) && dataEmissaoField.FieldType == DocumentFieldType.Date)
            {
                dataEmissao = dataEmissaoField.Content;
            }
            if (doc.Fields.TryGetValue("observacao", out var observacaoField) && observacaoField.FieldType == DocumentFieldType.String)
            {
                observacao = observacaoField.Content;
            }
            else
            {
                observacao = null!;
            }
        }

        return new CrlvInfo(
            Placa: placa,
            Renavam: long.Parse(renavam),
            EmissionDate: DateOnly.ParseExact(dataEmissao, "dd/MM/yyyy"),
            Obs: observacao);
    }

    public record CrlvInfo(
        string Placa,
        long Renavam,
        DateOnly EmissionDate,
        string? Obs);
}
