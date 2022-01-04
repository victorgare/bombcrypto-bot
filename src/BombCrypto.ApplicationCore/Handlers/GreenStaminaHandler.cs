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
        private const int MaxWaitTimeSeconds = 5;

        public async override Task HandleAsync(AutomationElement element)
        {
            var greenStaminaPathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "green-bar.png");
            var workButtonPathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "go-work.png");
            var greenStaminaTemplate = (Bitmap)Image.FromFile(greenStaminaPathTemplate);
            var workButtonTemplate = (Bitmap)Image.FromFile(workButtonPathTemplate);

            var retryCount = 0;
            var continueSearching = false;
            do
            {
                var source = ScreenCapture.CaptureWindow(element);
                var greenStaminaRectangles = ImageHelper.SearchBitmaps(greenStaminaTemplate, source);
                var workButtonRectangles = ImageHelper.SearchBitmaps(workButtonTemplate, source);

                // se houver uma imagem, continue procurando, caso contrario, pare
                continueSearching = greenStaminaRectangles.Any() && workButtonRectangles.Any();

                if (continueSearching)
                {
                    foreach (var fullStaminaRectangle in greenStaminaRectangles)
                    {
                        var bottonValue = fullStaminaRectangle.Bottom;
                        var nearestWorkButtonRectangle = workButtonRectangles.MinBy(x => System.Math.Abs((long)x.Bottom - bottonValue)).First();
                        var diff = bottonValue - nearestWorkButtonRectangle.Bottom;
                        if (diff <= 10 && diff >= -10)
                        {
                            var centerPoint = nearestWorkButtonRectangle.Center();
                            MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);
                        }
                    }

                }

                await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
            } while (retryCount <= MaxRetryCount && continueSearching);

            await base.HandleAsync(element);
        }
    }
}
