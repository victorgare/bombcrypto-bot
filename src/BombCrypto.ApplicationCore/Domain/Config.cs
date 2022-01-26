using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Domain
{
    public class Config
    {
        public int IntervalMinutes { get; set; } = 3;
        public AutomationElement Element { get; set; }
    }
}
