using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancer.Infrastructure
{
    public class Factory_Strategy
    {
        public static ILoadBalanceStrategy GetStrategy()
        {
            return new RoundRobin_Strategy();
        }
    }
}
