using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Newtonsoft.Json;
using RestSharp;

namespace EndToEndTests
{
    public class GraphClient
    {
        public static async Task<string> GetAccessToken()
        {
            var client = new RestClient("https://csc4151tak.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"2tq2oEkTlvAxU6uuiNS6hpduEQb0FF42\",\"client_secret\":\"E5eEYdeO7NAktEWBTk9Z82dJ_WlMGg2SMm8L6ZmauFf8wa0gVXfjKSCkWZjVD8bD\",\"audience\":\"https://tak\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var res = JsonConvert.DeserializeObject<Auth0Res>(response.Content);

            return res.access_token;
        }
    }

    public class Auth0Res
    {
        public string access_token;
    }
}
