using BombCrypto.ApplicationCore.Domain;
using System.Threading.Tasks;

namespace BombCrypto.ApplicationCore.Interfaces.Services
{
    public interface IProcessService
    {
        Task Process(Config config);
    }
}
