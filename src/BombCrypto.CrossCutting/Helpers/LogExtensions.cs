using Serilog;

namespace BombCrypto.CrossCutting.Helpers
{
    public static class LogExtensions
    {
        public static void ActionInformation(this Microsoft.Extensions.Logging.ILogger logger, string messageTemplate, params object[] args)
        {
            Log.ForContext("ActionInformation", true)
              .Information(messageTemplate, args);
        }
    }
}
