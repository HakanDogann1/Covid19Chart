using Covid19Chart.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Chart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);
            IQueryable<Covid> covidList = _covidService.GetList();
            return Ok(covidList);
        }
        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new Random();

            Enumerable.Range(1, 10).ToList().ForEach(async x =>
            {
                foreach (Ecity item in Enum.GetValues(typeof(Ecity)))
                {
                    var newCovid = new Covid
                    {
                        Ecity = item,
                        Count = random.Next(100, 1000),
                        CovidDate = DateTime.Now.AddDays(x)
                    };
                    _covidService.SaveCovid(newCovid).Wait();
                    System.Threading.Thread.Sleep(1000);
                }
            });
            return Ok("Covid 19 Dataları veri tabanına kaydedildi.");
        }
    }
}
