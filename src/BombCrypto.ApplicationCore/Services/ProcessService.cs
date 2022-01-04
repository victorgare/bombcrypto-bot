using BombCrypto.ApplicationCore.Handlers;
using BombCrypto.ApplicationCore.Interfaces.Services;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Services
{
    public class ProcessService : IProcessService
    {
        private readonly ConnectWallerHandler _abstractHandler;

        public ProcessService()
        {
            _abstractHandler = new ConnectWallerHandler();

            _abstractHandler.SetNext(new AcceptMetaMaskSignInHandler())
                            .SetNext(new HeroesScreenHandler());
        }

        public async Task Process(AutomationElement element)
        {
            await _abstractHandler.HandleAsync(element);
        }
    }
}
