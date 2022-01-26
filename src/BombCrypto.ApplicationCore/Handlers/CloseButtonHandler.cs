﻿using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class CloseButtonHandler : AbstractHandler
    {
        private const int MaxRetryCount = 5;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(Config config)
        {
            Console.WriteLine($"::CloseButtonHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("x.png");

            var retryCount = 0;
            var matched = false;
            do
            {
                var source = ScreenCapture.CaptureWindow(config.Element);
                var rectangle = ImageHelper.SearchBitmap(template, source);

                if (rectangle != Rectangle.Empty)
                {
                    var centerPoint = rectangle.Center();
                    MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                    matched = true;
                    Console.WriteLine($"::CloseButtonHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::CloseButtonHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }
            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::CloseButtonHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(config);
        }
    }
}
