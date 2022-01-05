using BombCrypto.ApplicationCore.Handlers;
using BombCrypto.ApplicationCore.Interfaces.Services;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Services
{
    public class ProcessService : IProcessService
    {
        private readonly OkHandler _abstractHandler;

        public ProcessService()
        {
            _abstractHandler = new OkHandler();

            _abstractHandler.SetNext(new ConnectWallerHandler())
                            .SetNext(new AcceptMetaMaskSignInHandler())
                            .SetNext(new NewMapHandler())
                            .SetNext(new GreenBackButtonHandler())
                            .SetNext(new HeroesScreenHandler())
                            .SetNext(new GreenStaminaHandler())
                            .SetNext(new CloseButtonHandler())
                            .SetNext(new TreasureHuntHandler());
        }

        public async Task Process(AutomationElement element)
        {
            await _abstractHandler.HandleAsync(element);
        }
    }
}
