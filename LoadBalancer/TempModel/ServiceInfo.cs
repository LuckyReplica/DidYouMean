using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancer.TempModel
{
    public class ServiceInfo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Priority { get; set; }

        public State ServiceState { get; set; }
    }

    public enum State { 
        Running,
        Paused
    }
}
