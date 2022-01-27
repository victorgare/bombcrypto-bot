using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class TreasureHuntHandler : AbstractHandler
    {
        private const int MaxRetryCount = 5;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(Config config)
        {
            Console.WriteLine($"::TreasureHuntHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("treasure-hunt-icon.png");
            var retryCount = 0;
            var matched = false;
            do
            {
                config.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                var source = ScreenCapture.CaptureWindow(config.Element);
                var rectangle = ImageHelper.SearchBitmap(template, source);

                if (rectangle != Rectangle.Empty)
                {
                    var centerPoint = rectangle.Center();
                    MouseOperations.DoubleClick(centerPoint.X, centerPoint.Y);

                    matched = true;

                    Console.WriteLine($"::TreasureHuntHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::TreasureHuntHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds), config.CancellationTokenSource.Token);
                }
            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::TreasureHuntHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(config);
        }
    }
}
