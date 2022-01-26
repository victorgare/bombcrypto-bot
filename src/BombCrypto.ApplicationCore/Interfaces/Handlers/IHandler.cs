using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Interfaces.Handlers
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        Task HandleAsync(AutomationElement element);

        Bitmap GetTemplate(string templateName);
    }
}
