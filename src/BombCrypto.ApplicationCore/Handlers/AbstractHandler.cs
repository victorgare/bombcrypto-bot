using BombCrypto.ApplicationCore.Interfaces.Handlers;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;

            // Returning a handler from here will let us link handlers in a
            // convenient way like this:
            // monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }

        public async virtual Task HandleAsync(AutomationElement element)
        {
            if (_nextHandler != null)
            {
                await _nextHandler.HandleAsync(element);
            }
        }
    }

}
