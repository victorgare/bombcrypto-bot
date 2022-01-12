using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using MoreLinq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class GreenStaminaHandler : AbstractHandler
    {
        private const int MaxRetryCount = 5;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(AutomationElement element)
        {
            Console.WriteLine($"::GreenStaminaHandler:: Iniciando - {DateTime.Now}");
            var greenStaminaPathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "green-bar.png");
            var workButtonPathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "go-work.png");
            var greenStaminaTemplate = (Bitmap)Image.FromFile(greenStaminaPathTemplate);
            var workButtonTemplate = (Bitmap)Image.FromFile(workButtonPathTemplate);

            var retryCount = 0;
            do
            {
                await ScrollDown(element);

                var source = ScreenCapture.CaptureWindow(element);

                var greenStaminaRectangles = ImageHelper.SearchBitmaps(greenStaminaTemplate, source);
                var workButtonRectangles = ImageHelper.SearchBitmaps(workButtonTemplate, source);

                if (greenStaminaRectangles.Any() && workButtonRectangles.Any())
                {
                    foreach (var greenStaminaRectangle in greenStaminaRectangles)
                    {
                        var bottonValue = greenStaminaRectangle.Bottom;
                        var nearestWorkButtonRectangle = workButtonRectangles.MinBy(x => System.Math.Abs((long)x.Bottom - bottonValue)).First();
                        var diff = bottonValue - nearestWorkButtonRectangle.Bottom;
                        if (diff <= 10 && diff >= -10)
                        {
                            var centerPoint = nearestWorkButtonRectangle.Center();
                            MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                            Console.WriteLine($"::GreenStaminaHandler:: Encontrado - {DateTime.Now}");
                            retryCount--;
                        }
                    }
                }

                retryCount++;

                Console.WriteLine($"::GreenStaminaHandler:: Tentativa {retryCount} - {DateTime.Now}");
                await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
            } while (retryCount <= MaxRetryCount);

            Console.WriteLine($"::GreenStaminaHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(element);
        }

        private async Task ScrollDown(AutomationElement element)
        {
            var bottonFramePathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "heroes-list-botton-frame.png");
            var bottonFrameTemplate = (Bitmap)Image.FromFile(bottonFramePathTemplate);

            var source = ScreenCapture.CaptureWindow(element);

            var bottonFrameRectangle = ImageHelper.SearchBitmap(bottonFrameTemplate, source);

            if (bottonFrameRectangle != Rectangle.Empty)
            {

                var startX = bottonFrameRectangle.Center().X;
                var startY = bottonFrameRectangle.Top - 100;

                MouseOperations.Scroll(startX, startY, -1200);

                await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
            }
        }
    }
}
