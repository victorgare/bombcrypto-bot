﻿using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class HeroesScreenHandler : AbstractHandler
    {
        private const int MaxRetryCount = 20;
        private const int MaxWaitTimeSeconds = 5;

        public async override Task HandleAsync(AutomationElement element)
        {
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "hero-icon.png");
            var template = (Bitmap)Image.FromFile(pathTemplate);

            var retryCount = 0;
            var matched = false;
            do
            {
                var source = ScreenCapture.CaptureWindow(element);
                var rectangle = ImageHelper.SearchBitmap(template, source);

                if (rectangle != Rectangle.Empty)
                {
                    var centerPoint = rectangle.Center();
                    MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                    matched = true;
                }

                if (!matched)
                {
                    retryCount++;
                }

                await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
            } while (retryCount <= MaxRetryCount && !matched);

            await base.HandleAsync(element);
        }
    }
}