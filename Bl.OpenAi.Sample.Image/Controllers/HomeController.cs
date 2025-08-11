using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bl.OpenAi.Sample.Image.Models;
using Bl.OpenAi.Sample.Image.Services;

namespace Bl.OpenAi.Sample.Image.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ImageGeneratorService _imageService;

    public HomeController(ILogger<HomeController> logger, ImageGeneratorService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GenerateImage(ImageGenerationModel model)
    {
        if (string.IsNullOrEmpty(model.Prompt)) return BadRequest("Model cannot be null or empty.");

        var imageResponse = await _imageService.GenerateAsync(model.Prompt);

        var result = new ImageGenerationModel
        {
            Prompt = model.Prompt,
            RevisedPrompt = imageResponse.RevisedPrompt,
            ImageUrl = imageResponse.ImageUri.AbsoluteUri
        };

        return Json(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
