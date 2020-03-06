using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadBalancer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace LoadBalancer.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class LoadBalancerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger<LoadBalancerController> _logger;

        public LoadBalancerController(ILogger<LoadBalancerController> logger, ILoadBalanceStrategy strategy)
        {
            _logger = logger;
            _Strategy = strategy;
        }

        private ILoadBalanceStrategy _Strategy;

        private static int urlIndex = 0;

        [HttpGet]
        [Route("GetWords/{keyWord}/{distance}")]
        public IEnumerable<string> Get(string keyWord, int distance)
        {
            return LoadBalanceAsync(keyWord, distance).Result;
        }

        private async Task<IEnumerable<string>> LoadBalanceAsync(string keyWord, int distance)
        {
            RestClient c = new RestClient();
            c.BaseUrl = new Uri(_Strategy.BalanceUrl());//new Uri(Urls[urlIndex % 2]);
            urlIndex += 1;

            if (urlIndex == 2)
            {
                urlIndex = 1;
            }

            var request = new RestRequest(Method.GET);

            request.AddParameter("keyWord", keyWord);
            request.AddParameter("distance", distance.ToString());

            foreach (string text in Summaries)
            {
                request.AddParameter("words", text);
            }

            var response = await c.ExecuteAsync<IEnumerable<string>>(request);
            return response.Data;
        }
    }
}
