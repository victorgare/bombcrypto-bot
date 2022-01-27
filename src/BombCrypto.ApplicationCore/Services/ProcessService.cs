using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ApplicationCore.Handlers;
using BombCrypto.ApplicationCore.Interfaces.Services;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Services
{
    public class ProcessService : IProcessService
    {
        private readonly OkHandler _abstractHandler;

        public ProcessService()
        {
            _abstractHandler = new OkHandler();

            _abstractHandler.SetNext(new ConnectWallerHandler())
                            .SetNext(new AcceptMetaMaskSignInHandler())
                            .SetNext(new NewMapHandler())
                            .SetNext(new GreenBackButtonHandler())
                            .SetNext(new HeroesScreenHandler())
                            .SetNext(new GreenStaminaHandler())
                            .SetNext(new CloseButtonHandler())
                            .SetNext(new TreasureHuntHandler());
        }

        public async Task Process(Config config)
        {
            try
            {
                do
                {
                    var browsers = BrowserDetector.GetBrowses();
                    foreach (var browser in browsers)
                    {
                        // the chrome process must have a window 
                        if (browser.MainWindowHandle == IntPtr.Zero)
                        {
                            continue;
                        }

                        // to find the tabs we first need to locate something reliable - the 'New Tab' button 
                        AutomationElement root = AutomationElement.FromHandle(browser.MainWindowHandle);

                        // loop through all the tabs and get the names which is the page title 
                        Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                        foreach (AutomationElement tabitem in root.FindAll(TreeScope.Descendants, condTabItem))
                        {
                            if (tabitem.Current.Name.Equals("Bombcrypto"))
                            {
                                tabitem.SetFocus();

                                var handler = browser.MainWindowHandle;
                                ScreenHelper.MoveWindow(handler, 0, 0, 1366, 768, false);
                                ScreenHelper.SetWindowPos(handler, IntPtr.Zero, 10, 10, 1366, 768, ScreenHelper.SetWindowPosFlags.SWP_SHOWWINDOW);
                                ScreenHelper.MoveWindow(handler, 10, 10, 1366, 768, false);

                                Console.WriteLine($"Iniciando robo - {DateTime.Now}");
                                config.Element = root;

                                await _abstractHandler.HandleAsync(config);
                                Console.WriteLine($"Fim do robo - {DateTime.Now}");
                            }
                        }
                    }

                    // aguarde o tempo do config em minutos e faca novamente o processo
                    Console.WriteLine($"Aguardando: {config.IntervalMinutes} minutos");
                    await Task.Delay(TimeSpan.FromMinutes(config.IntervalMinutes), config.CancellationTokenSource.Token);
                } while (!config.CancellationTokenSource.IsCancellationRequested);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operacao cancelada");
            }

        }
    }
}
