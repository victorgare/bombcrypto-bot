using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class NewMapHandler : AbstractHandler
    {
        public async override Task HandleAsync(Config config)
        {
            Console.WriteLine($"::NewMapHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("new-map.png");
            var source = ScreenCapture.CaptureWindow(config.Element);
            var rectangle = ImageHelper.SearchBitmap(template, source);

            if (rectangle != Rectangle.Empty)
            {
                var centerPoint = rectangle.Center();
                MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                Console.WriteLine($"::NewMapHandler:: Encontrado - {DateTime.Now}");
            }

            Console.WriteLine($"::NewMapHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(config);
        }
    }
}
