using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace EndToEndTests
{
    public class PingTests
    {
        private string tok = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IlMyLVdiZlVXWDh5S2tnbUxWYzk0WiJ9.eyJpc3MiOiJodHRwczovL2NzYzQxNTF0YWsuYXV0aDAuY29tLyIsInN1YiI6Imdvb2dsZS1vYXV0aDJ8MTEwNzYxNjI1MjE1MDE2MDEwNjQ1IiwiYXVkIjpbImh0dHBzOi8vdGFrIiwiaHR0cHM6Ly9jc2M0MTUxdGFrLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE1ODkzOTc0MzMsImV4cCI6MTU4OTQ4MzgzMywiYXpwIjoibGJweVhMM1I5Q1pSNWgxM1Z0ZDNFQnlnYzUxU0FpTE8iLCJzY29wZSI6Im9wZW5pZCJ9.VxVaEfqKQsMZtLOS-V9AS92ZjtOhNMGxRfIhSQbPNvsQPKYsGeDXK3QX84fMms676fPjt3kmD6Ah-PjKfB7XdiFIgvvbbKUMCbNgERVyAcpGlBxuarcz8c0zomNwlgzqjFYDmsefV1PO9av7W-lNlelTbNyQBknN1BCTsik5VbYrNPfVLJW_T0gjx5AZPIEnQgY2L7xuiVCZvEarjxCYIMYMS0izwnB54oDTA_u1YkOHQx2WG1-eyAqySHbaUHRiojugPw9YhE_-aPWqQktffijObRtJN1Gjl5O5r5zI-p8bey8efriBsMJOYJJVv_yePADnGm_Vlr2TtDXffDQupg";
            
        [Fact]
        public async Task PingTest()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var token = await GraphClient.GetAccessToken();

            //var httpClient = new HttpClient() { BaseAddress = new Uri("https://takkapp.azurewebsites.net") };
            var httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:54381/") };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tok);

            var res = await httpClient.GetAsync($"Ping");
            
            Assert.True(res.IsSuccessStatusCode);

            var body = await res.Content.ReadAsStringAsync();

            Assert.True(body == "Pong");
        }
    }
}
