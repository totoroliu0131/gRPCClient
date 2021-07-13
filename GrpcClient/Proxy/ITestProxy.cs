using System.Threading.Tasks;
using GrpcGreeterClient;

namespace GrpcClient.Proxy
{
    public interface ITestProxy
    {
        Task<HelloReply> TestPost(HelloRequest helloRequest);
    }
}