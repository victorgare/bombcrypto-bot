using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class OkHandler : AbstractHandler
    {
        private const int MaxRetryCount = 2;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(Config config)
        {
            Console.WriteLine($"::OkHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("ok.png");

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
                    MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);
                    matched = true;
                    Console.WriteLine($"::OkHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::OkHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds), config.CancellationTokenSource.Token);
                }

            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::OkHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(config);
        }
    }
}
