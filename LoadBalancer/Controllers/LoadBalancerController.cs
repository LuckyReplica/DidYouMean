using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private static readonly string[] Urls = new[]
        {
            "https://localhost:44341/getWords", "https://localhost:44377/getWords"
        };

        private readonly ILogger<LoadBalancerController> _logger;

        public LoadBalancerController(ILogger<LoadBalancerController> logger)
        {
            _logger = logger;
        }

        private static int urlIndex = 0;

        [HttpGet]
        [Route("UseAsync")]
        public async Task<IEnumerable<string>[]> Get()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44391/GetWordsAsync");

            var request1 = new RestRequest(Method.GET);
            var request2 = new RestRequest(Method.GET);
            var request3 = new RestRequest(Method.GET);

            request1.AddParameter("keyWord", "Freezing");
            request1.AddParameter("distance", 2);

            request2.AddParameter("keyWord", "Blazing");
            request2.AddParameter("distance", 1);

            request3.AddParameter("keyWord", "Mild");
            request3.AddParameter("distance", 3);

            Task<IEnumerable<string>> task1 = ProcessURLAsync(request1, client);
            Task<IEnumerable<string>> task2 = ProcessURLAsync(request2, client);
            Task<IEnumerable<string>> task3 = ProcessURLAsync(request3, client);
            var result1 = await task1;
            var result2 = await task2;
            var result3 = await task3;

            await Task.WhenAll(new[] { task1, task2, task3 });


            return await Task.WhenAll(new[] { task1, task2, task3 });
        }

        [HttpGet]
        [Route("GetWordsAsync")]
        public IEnumerable<string> AsyncGet(string keyWord, int distance)
        {
            RestClient c = new RestClient();
            c.BaseUrl = new Uri(Urls[urlIndex % 2]);
            urlIndex += 1;

            var request = new RestRequest(Method.GET);

            request.AddParameter("keyWord", keyWord);
            request.AddParameter("distance", distance.ToString());

            foreach (string text in Summaries)
            {
                request.AddParameter("words", text);
            }

            var response = c.ExecuteAsync<IEnumerable<string>>(request);
            return response.Result.Data;

        }

        [HttpGet]
        [Route("GetWords/{keyWord}/{distance}")]
        public IEnumerable<string> Get(string keyWord, int distance)
        {
            RestClient c = new RestClient();
            c.BaseUrl = new Uri(Urls[urlIndex % 2]);
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

            var response = c.Execute<IEnumerable<string>>(request);
            return response.Data;

        }

        async Task<IEnumerable<string>> ProcessURLAsync(RestRequest request, RestClient client)
        {
            var response = await client.ExecuteAsync<IEnumerable<string>>(request);

            return response.Data;
        }
    }
}
