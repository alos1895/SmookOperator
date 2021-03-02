using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmookOperator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmookOperator.Controllers
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
        [Route("/detail")]
        public IActionResult Detalle()
        {
            return View();
        }

        [HttpGet("/detailLastMeasure/{deviceId}")]
        public async Task<IActionResult> GETDetailDevice(string deviceId)
        {
            try
            {
                var httpPeticion = (HttpWebRequest)WebRequest.Create($"http://volcani.herokuapp.com/find-one/{deviceId}");
                httpPeticion.ContentType = "application/json;charset=utf-8";
                httpPeticion.Method = "GET";

                var httpRespuesta = (HttpWebResponse)httpPeticion.GetResponse();
                string results;

                var detailDevice = new DetailDevice();
                using (var lector = new StreamReader(httpRespuesta.GetResponseStream()))
                {
                    results = await lector.ReadToEndAsync();
                }
                detailDevice = JsonConvert.DeserializeObject<DetailDevice>(results);

                return Ok(detailDevice);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
