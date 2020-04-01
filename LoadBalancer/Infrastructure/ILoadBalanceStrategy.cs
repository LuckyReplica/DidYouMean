﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancer.Infrastructure
{
    public interface ILoadBalanceStrategy
    {
        public void Initialize();

        string BalanceUrl();
    }
}
