using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancer.Infrastructure
{
    public class RoundRobin_Strategy : ILoadBalanceStrategy
    {

        private static readonly string[] Urls = new[]
{
            "https://localhost:44341/getWords", "https://localhost:44377/getWords"
        };

        private static int index = 0;

        public string BalanceUrl()
        {
            var url = Urls[index % 2];
            index++;

            if (index == 2)
            {
                index = 1;
            }

            return url;
        }
    }
}
