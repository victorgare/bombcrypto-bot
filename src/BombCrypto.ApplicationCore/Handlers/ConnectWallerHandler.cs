using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class ConnectWallerHandler : AbstractHandler
    {

        public async override Task HandleAsync(AutomationElement element)
        {
            var source = ScreenCapture.CaptureWindow(element);
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "connect-wallet.png");
            var template = (Bitmap)Image.FromFile(pathTemplate);

            var rectangle = ImageHelper.SearchBitmap(template, source);

            if (rectangle != Rectangle.Empty)
            {
                var centerPoint = rectangle.Center();
                MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);
            }

            await base.HandleAsync(element);
        }
    }
}
