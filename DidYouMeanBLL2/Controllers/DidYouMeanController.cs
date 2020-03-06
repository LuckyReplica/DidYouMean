using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DidYouMeanBLL.Controllers
{
    [ApiController]
    public class DidYouMeanController : ControllerBase
    {
        private static readonly string guid = Guid.NewGuid().ToString();

        [HttpGet]
        [Route("getWords")]
        public IEnumerable<string> Get(string keyWord, int distance, [FromQuery]string[] words)
        {
            List<string> output = new List<string>();
            output.Add(guid);

            int? pos = Array.IndexOf(words, keyWord);
            if (pos != null && pos >= 0)
            {
                int offSet = (int)pos;

                for (int i = 0; i <= distance; i++)
                {
                    output.Add(words[i + offSet]);
                }
            }

            return output.ToArray();
        }
    }
}
