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
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "Connect_Wallet.png");
            var template = (Bitmap)Image.FromFile(pathTemplate);

            var images = ImageHelper.FindImage(source, template);

            foreach (var image in images)
            {
                MouseOperations.MouseClick(image.X, image.Y);
            }

            await base.HandleAsync(element);
        }
    }
}
