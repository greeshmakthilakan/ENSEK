using ENSEKAPIConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;

namespace ENSEKAPIConsume.Controllers
{
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public ViewResult AddFile() => View("Index");
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                using (var fileStream = file.OpenReadStream())
                {
                    var file1 = new StreamContent(fileStream);
                    file1.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    form.Add(file1, "formFiles", file.FileName);
                    using (var response = await httpClient.PostAsync("https://localhost:7289/API/ENSEK/meter-reading-uploads", form))
                    {
                        if(response.IsSuccessStatusCode)
                        {
                            response.EnsureSuccessStatusCode();
                            var apiResponse = response.Content.ReadAsStringAsync();
                            var viewModel = JsonConvert.DeserializeObject<Counts>(apiResponse.Result);
                            return View(viewModel);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Something went wrong.";
                            return View();
                        }
                    }


                }
            }
            
        }
    }
}