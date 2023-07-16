using Covid19Chart.Api.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Covid19Chart.Api.Hubs
{
    public class CovidHub:Hub
    {
        private readonly CovidService _covidService;

        public CovidHub(CovidService covidService)
        {
            _covidService = covidService;
        }

        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", _covidService.GetCovidChartList());
        }
    }
}
