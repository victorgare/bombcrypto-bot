using System.Threading;

namespace BombCrypto.CrossCutting.Helpers
{
    public class CencelationTokenHelper
    {
        private static CancellationTokenSource _cancellationTokenSource = null;

        public static CancellationTokenSource CancellationTokenSource
        {
            get
            {
                if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }

                return _cancellationTokenSource;
            }
        }

    }
}
