using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrpcGreeterClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GrpcClient.Proxy
{
    public class TestProxy : ITestProxy
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public TestProxy(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _url = configuration["GrpcServer"];
        }
        public async Task<HelloReply>TestPost(HelloRequest helloRequest)
        {
            var requestContent = JsonConvert.SerializeObject(helloRequest);
            var stringContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
            
            var responseRaw = await _httpClient.PostAsync($"{_url}/Grpc/Get", stringContent); 
            
            var response = await responseRaw.Content.ReadAsAsync<HelloReply>();

            return response;
        }
    }
}