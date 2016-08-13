using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Historian.Api.Controllers
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
            // get messages
            var messages = _logRetriever.GetMessages(channel);

            // order by timestamp
            messages = messages.OrderByDescending(m => m.Timestamp);

            // take number provided
            return messages.Take(number);
        }

        [HttpGet]
        [Route("channels/{channel}/messages/last-day/{kind}")]
        public IEnumerable<Message> LastDay(string channel, string kind = "All")
        {
            // get messages
            var messages = _logRetriever.GetMessages(channel);

            // get day period
            var endPeriod = DateTime.Now;
            var startPeriod = endPeriod.AddDays(-1);

            // get messages in period
            messages = messages.Where(m => m.Timestamp >= startPeriod && m.Timestamp <= endPeriod);
            if (kind != "All")
            {
                var kindEnum = (MessageKind) Enum.Parse(typeof(MessageKind), kind);
                messages = messages.Where(m => m.Kind == kindEnum);
            }

            // return messages by timestamp
            return messages.OrderByDescending(m => m.Timestamp);
        }

        [HttpGet]
        [Route("channels/{channel}/graphs/last-twelve-hours")]
        public dynamic LastTwelveHours(string channel)
        {
            // setup for retrieval
            var totalHours = 12;
            var hours = new List<int>();
            var series = new List<dynamic>();
            var currentDate = DateTime.Now;
            var periodEnd = currentDate;
            var periodStart = periodEnd.AddHours(-totalHours);
            var currentHour = currentDate.Hour + 2;
            var endHour = currentHour;
            var startHour = currentHour - totalHours;

            // set start of period
            if (startHour < 0)
            {
                startHour = 24 - startHour;
            }

            // add hours for labels
            var hourCounter = startHour;
            for (var i = 0; i < totalHours; i++)
            {
                if (hourCounter >= 24) hourCounter = 0;
                hours.Add(hourCounter);

                hourCounter++;
            }

            // get messages
            var messages = _logRetriever.GetMessages(channel);

            // get messages for period
            var messagesLastPeriod = messages.Where(m => m.Timestamp >= periodStart && m.Timestamp <= periodEnd);

            // group by kind
            var groupedByKind = messagesLastPeriod.GroupBy(m => m.Kind);

            // go through kinds
            foreach (var gKind in groupedByKind)
            {
                // get all for kind grouped by hour
                var byHour = gKind.GroupBy(m => m.Timestamp.Hour);

                // create current dataset
                var currentSeries = new List<int>();
                foreach (var hour in hours)
                {
                    var messageCount = 0;
                    var hGroup = byHour.FirstOrDefault(g => g.Key == hour);
                    if (hGroup != null) messageCount = hGroup.Count();
                    currentSeries.Add(messageCount);
                }

                // add series data
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

            // return graph data
            return new
            {
                labels = hours.Select(t => string.Format("{0:D2}:00", t)),
                datasets = series
            };
        }
    }
}
