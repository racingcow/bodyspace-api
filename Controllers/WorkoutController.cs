using System;
using System.Net;
using System.Net.Http;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

//using bodyspace_api.Models;

namespace bodyspace_api.Controllers
{
    [Route("api/[controller]")]
    public class WorkoutController : Controller
    {
        //could we move this to a base class /singleton for all controllers?
        private static CookieContainer _cookieContainer = new CookieContainer();
        private static HttpClientHandler _clientHandler = new HttpClientHandler() { CookieContainer = _cookieContainer };
        private static HttpClient _client = new HttpClient(_clientHandler);
        private readonly ILogger _logger;

        public WorkoutController(ILogger<WorkoutController> logger)
        {
            _logger = logger;
        }

        // GET api/Workout
        [HttpGet]
        //public Workout[] Get(DateTime fromDate)
        public string Get(string token, DateTime fromDate)
        {
            var formattedDate = fromDate.ToString("MMM d, yyyy");
            var stringContent = $"workoutLogs:dateRangePanel:wdatefrom={formattedDate}";
            var encodedStringContent = WebUtility.UrlEncode(stringContent);
            var content = new StringContent(encodedStringContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var url = "http://my.bodybuilding.com/workouts/workout-logs/racingcow?2-1.IBehaviorListener.1-workoutLogs-dateRangePanel-wdatefrom";

            //super nervous about this being static. need to test in multiple user scenarios to make sure tokens are getting shared.
            var cookie = new Cookie("v1guid", token);
            _cookieContainer.Add(new Uri("http://my.bodybuilding.com"), cookie);

            using (var res = _client.PostAsync(url, content).Result)
            {
                var resultString = res.Content.ReadAsStringAsync().Result;
                return resultString;
            }
        }
    }
}
