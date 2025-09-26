using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using vision_api_sample_post.Models;
using vision_api_sample_post.Service;

namespace vision_api_sample_post.Controllers
{
    public class HomeController : Controller
    {
        private readonly VisionAPIService _visionService;

        public HomeController(VisionAPIService visionAPIService)
        {
            _visionService = visionAPIService;
        }

        public IActionResult Index()
        {
            var json = System.IO.File.ReadAllText("mockapi.json");
            var functions = JsonSerializer.Deserialize<List<FunctionDef>>(json);
            return View(functions);
        }

        [HttpPost]
        public async Task<IActionResult> Test(string requestJson)
        {
            var response = await _visionService.AnalyzeImageAsync(requestJson);

            return Json(new
            {
                response = response
            });
        }
    }
}
