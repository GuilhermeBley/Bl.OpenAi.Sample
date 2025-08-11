using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bl.OpenAi.Sample.Image.Models;

namespace Bl.OpenAi.Sample.Image.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
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
        var result = new ImageGenerationModel
        {
            Prompt = model.Prompt,
            RevisedPrompt = $"Enhanced version of: {model.Prompt}",
            ImageUrl = $"https://placehold.co/600x400?text={Uri.EscapeDataString(model.Prompt)}"
        };

        return Json(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
