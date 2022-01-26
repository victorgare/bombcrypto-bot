using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ApplicationCore.Services;
using System.Threading.Tasks;

namespace BombCrypto.ConsoleApplication
{
    public class Program
    {
        async static Task Main()
        {
            var processService = new ProcessService();
            var config = new Config();
            await processService.Process(config);
        }
    }
}
