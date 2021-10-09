using Domain.External;
using Newtonsoft.Json;
using RestSharp;

namespace Services
{
    public class LocationHandler
    {
        private string IpStackBaseUrl = "http://api.ipstack.com/{ip}";
        private string IpStackAccessKey = "dfb08ef3ce7e15c4adf897aa09dd7e37"; // todo: move to config

        public LocationHandler()
        {
        }

        public string GetLocationByIP(string ip)
        {
            // use IP as input to REST service to get location
            // return location

            var client = new RestClient(IpStackBaseUrl);

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("ip", ip);
            request.AddQueryParameter("access_key", IpStackAccessKey);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var data = JsonConvert.DeserializeObject<IpStackResponse>(response.Content);

                return GenerateLocationString(data);
            }

            return null;

            string GenerateLocationString(IpStackResponse ipStackResponse)
            {
                string result = string.Empty;

                if (ipStackResponse.City != null)
                    result += $"{ipStackResponse.City}, ";

                if (ipStackResponse.CountryName != null)
                    result += ipStackResponse.CountryName;

                return result;
            }
        }
    }
}
