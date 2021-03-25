using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Website.Models;

namespace WebSite.Controllers
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
            var modelList = GetListWeatherForecast();
            return View(modelList);
        }

        private List<WeatherForecastVM> GetListWeatherForecast()
        {
            var modelList = new List<WeatherForecastVM>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://backend/");

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                var responseTask = client.GetAsync("api/WeatherForecast/List");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                    modelList = JSserializer.Deserialize<List<WeatherForecastVM>>(data);
                }
            }

            return modelList;
        }
    }
}
