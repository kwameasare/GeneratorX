using GeneratorXWeb.Generators;
using GeneratorXWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace GeneratorXWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelGenerator modelGenerator;
        private readonly ServiceGenerator serviceGenerator;
        private readonly RepositoryGenerator repositoryGenerator;
        public HomeController(ILogger<HomeController> logger, ModelGenerator modelGenerator, ServiceGenerator serviceGenerator, RepositoryGenerator repositoryGenerator)
        {
            this.modelGenerator = modelGenerator;
            _logger = logger;
            this.serviceGenerator = serviceGenerator;
            this.repositoryGenerator = repositoryGenerator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddFeature(string featureNameList)
        {
            var deserializedJson = JsonSerializer.Deserialize<List<string>>(featureNameList!);
            var viewModel = new FeatureViewModel()
            {
                FeatureNameList = deserializedJson
            };
            return PartialView(viewModel);
        }

        [HttpPost]
        public IActionResult GetGridList(string featureList)
        {
            if (featureList != null || featureList != "[]")
            {
                var deserializedJson = JsonSerializer.Deserialize<List<FeatureViewModel>>(featureList!);
                return PartialView(deserializedJson);
            }
            return PartialView();

        }
        public IActionResult AddProject(string project)
        {
            if (ModelState.IsValid)
            {
                var deserializedJson = JsonSerializer.Deserialize<ProjectViewModel>(project);
                modelGenerator.GenerateModel(deserializedJson!);
                serviceGenerator.GenerateService(deserializedJson!);
                repositoryGenerator.GenerateRepository(deserializedJson!);
                return Content("Success");
            }
            throw new Exception("Failed");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}