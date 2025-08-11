using Azure;
using Azure.AI.OpenAI;
using OpenAI.Images;

namespace Bl.OpenAi.Sample.Image.Services;

public class ImageGeneratorService
{
    private readonly ImageClient _imageClient;

    public ImageGeneratorService(IConfiguration configuration)
    {
        var endpoint = configuration["endpoint"];
        var key = configuration["key"];
        var model = "dall-e-3";

        if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Configuration for OpenAI endpoint or client is missing.");
        }

        var client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
        _imageClient = client.GetImageClient(model);
    }

    public async Task<GeneratedImage> GenerateAsync(string prompt)
    {
        var result = await _imageClient.GenerateImageAsync(prompt, new()
        {
            Size = GeneratedImageSize.W1024xH1024,
        });

        return result;
    }
}
