using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcClient.Proxy;
using GrpcGreeterClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrpcClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly Greeter.GreeterClient _client;
        private readonly ITestProxy _proxy;
        private string _server;

        public TestController(ILogger<TestController> logger,Greeter.GreeterClient client,ITestProxy proxy,IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            _proxy = proxy;
            _server = configuration["GrpcServer"];
        }

        [HttpGet]
        [Route("GrpcTest")]
        public async Task<HelloReply> GrpcTest()
        {
            using var grpcChannel = GrpcChannel.ForAddress(_server);
            var client = new Greeter.GreeterClient(grpcChannel);

            var logDate = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime());
            var response = await client.SayHelloAsync(new HelloRequest
            {
                Name = "Benjamin",
                LogDate = logDate
            });
            
            return response;
        }
        
        [HttpGet]
        [Route("ControllerTest")]
        public async Task<HelloReply> ControllerTest()
        {
            var logDate = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime());
            var helloRequest = new HelloRequest
            {
                Name = "Benjamin",
                LogDate = logDate 
            };
            var response = await _proxy.TestPost(helloRequest);

            return response;
        }
    }
}