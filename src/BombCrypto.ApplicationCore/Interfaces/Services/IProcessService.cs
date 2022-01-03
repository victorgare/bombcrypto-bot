using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Interfaces.Services
{
    public interface IProcessService
    {
        Task Process(AutomationElement element);
    }
}
