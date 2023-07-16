using Covid19Chart.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Chart.Api.Models
{
    public class CovidService
    {
        private readonly Context _context;
        private readonly IHubContext<CovidHub> _hubContext;
        public CovidService(Context context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new List<CovidChart>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select Tarih , [1],[2],[3],[4],[5] from\r\n(select [Ecity],[Count],Cast([CovidDate] as date) as Tarih From Covids) as CovidT\r\nPIVOT\r\n(SUM(Count) for Ecity IN([1],[2],[3],[4],[5])) as PTable";

                command.CommandType = System.Data.CommandType.Text;
                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart covidChart = new CovidChart();
                        covidChart.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                covidChart.Counts.Add(0);
                            }
                            else
                            {
                                covidChart.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidCharts.Add(covidChart);
                    }
                }
                _context.Database.CloseConnection();
                return covidCharts;
            }
        }

    }
}
