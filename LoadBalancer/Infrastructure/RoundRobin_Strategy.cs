using LoadBalancer.TempModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace LoadBalancer.Infrastructure
{
    public class RoundRobin_Strategy : ILoadBalanceStrategy
    {

        private ServiceInfo.ServiceInfo[] Urls;

        private static int index = 0;

        public string BalanceUrl()
        {
            Urls = GetServices().ToArray();
            if (Urls.Length < 1)
            {
                throw new Exception("There are no services in the repository.");
            }

            if (index >= Urls.Length)
            {
                index = 0;
            }

            var url = Urls[index];
            int count = 0;

            while (url.ServiceState == ServiceInfo.ServiceInfo.State.Paused)
            {
                if (count == Urls.Length)
                {
                    throw new InvalidOperationException("No services are currently running");
                }
                index++;
                url = Urls[index];

                count++;
            }
            index++;

            return url.Url;
        }

        private IEnumerable<ServiceInfo.ServiceInfo> GetServices()
        {
            RestSharp.RestClient client = new RestClient();

            client.BaseUrl = new Uri("https://localhost:44383/services");
            var request = new RestRequest(Method.GET);

            var response = client.Execute<IEnumerable<ServiceInfo.ServiceInfo>>(request);

            Urls = response.Data.ToArray();

            return Urls;
        }


        public void Initialize()
        {
            //Insert the call to the new service here to make it viable for using in our round robin strategy
            //var info1 = new ServiceInfo() { Id = 1, Priority = 1, ServiceState = State.Running, Url = "https://localhost:44341/getWords" };
            //var info2 = new ServiceInfo() { Id = 2, Priority = 2, ServiceState = State.Running, Url = "https://localhost:44377/getWords" };

            RestSharp.RestClient client = new RestClient();

            client.BaseUrl = new Uri("https://localhost:44383/services");
            var request = new RestRequest(Method.GET);

            var response = client.Execute<IEnumerable<ServiceInfo.ServiceInfo>>(request);

            Urls = response.Data.ToArray();
        }
    }
}
