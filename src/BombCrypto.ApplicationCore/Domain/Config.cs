using System.Threading;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Domain
{
    public class Config
    {
        public int IntervalMinutes { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public AutomationElement Element { get; set; }
    }
}
