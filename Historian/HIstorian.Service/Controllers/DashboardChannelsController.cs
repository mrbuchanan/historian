using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Historian.Service.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardChannelsController : ApiController
    {
        private readonly ILogger _logger;
        private readonly ILogRetriever _logRetriever;

        public DashboardChannelsController(ILogger logger, ILogRetriever logRetriever)
        {
            _logger = logger;
            _logRetriever = logRetriever;
        }

        [HttpGet]
        [Route("channels/{channel}/messages/mostRecent/{number}")]
        public IEnumerable<Message> MostRecent(string channel, int number)
        {
            var messages = _logRetriever.GetMessages(channel);
            messages = messages.OrderByDescending(m => m.Timestamp);
            return messages.Take(number);
        }

        [HttpGet]
        [Route("channels/{channel}/messages/last-day")]
        public IEnumerable<Message> LastDay(string channel)
        {
            var messages = _logRetriever.GetMessages(channel);

            var endPeriod = DateTime.Now;
            var startPeriod = endPeriod.AddDays(-1);

            messages = messages.Where(m => m.Timestamp >= startPeriod && m.Timestamp <= endPeriod);

            return messages.OrderByDescending(m => m.Timestamp);
        }

        [HttpGet]
        [Route("channels/{channel}/graphs/last-twelve-hours")]
        public dynamic LastTwelveHours(string channel)
        {
            var totalHours = 12;
            var hours = new List<int>();
            var series = new List<dynamic>();
            var currentDate = DateTime.Now;
            var periodEnd = currentDate;
            var periodStart = periodEnd.AddHours(-totalHours);
            var currentHour = currentDate.Hour;
            var endHour = currentHour;
            var startHour = currentHour - totalHours;
            if (startHour < 0)
            {
                startHour = 24 - startHour;
            }

            var hourCounter = startHour;
            for (var i = 0; i < totalHours; i++)
            {
                if (hourCounter >= 24) hourCounter = 0;
                hours.Add(hourCounter);

                hourCounter++;
            }

            var messages = _logRetriever.GetMessages(channel);
            var messagesLastPeriod = messages.Where(m => m.Timestamp >= periodStart && m.Timestamp <= periodEnd);
            var groupedByKind = messagesLastPeriod.GroupBy(m => m.Kind);

            foreach (var gKind in groupedByKind)
            {
                var byHour = gKind.GroupBy(m => m.Timestamp.Hour);
                var currentSeries = new List<int>();
                foreach (var hour in hours)
                {
                    var messageCount = 0;
                    var hGroup = byHour.FirstOrDefault(g => g.Key == hour);
                    if (hGroup != null) messageCount = hGroup.Count();
                    currentSeries.Add(messageCount);
                }

                series.Add(new
                {
                    label = gKind.Key.ToString(),
                    data = currentSeries,
                    backgroundColor = ChartHelper.GetColourByKind(gKind.Key, 0.4M),
                    borderColor = ChartHelper.GetColourByKind(gKind.Key, 1.0M),
                    pointBorderColor = ChartHelper.GetColourByKind(gKind.Key, 1.0M),
                    pointHoverBackgroundColor = ChartHelper.GetColourByKind(gKind.Key,1.0M),
                    pointHoverBorderColor = ChartHelper.GetColourByKind(gKind.Key, 1.0M),
                });
            }

            return new
            {
                labels = hours.Select(t => string.Format("{0:D2}:00", t)),
                datasets = series
            };
        }
    }
}
