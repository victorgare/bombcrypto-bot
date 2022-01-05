using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class NewMapHandler : AbstractHandler
    {
        public async override Task HandleAsync(AutomationElement element)
        {
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "new-map.png");
            var template = (Bitmap)Image.FromFile(pathTemplate);

            var source = ScreenCapture.CaptureWindow(element);
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
