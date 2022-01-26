using BombCrypto.ApplicationCore.Interfaces.Handlers;
using System;
using System.Drawing;
using System.IO;
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

        public Bitmap GetTemplate(string templateName)
        {
            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", templateName);
            // var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "ok.png");
            if (File.Exists(pathTemplate))
            {
                return (Bitmap)Image.FromFile(pathTemplate);
            }

            pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", templateName);

            return (Bitmap)Image.FromFile(pathTemplate);
        }
    }

}
