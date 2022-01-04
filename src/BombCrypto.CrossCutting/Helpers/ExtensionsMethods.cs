using System;

namespace BombCrypto.CrossCutting.Helpers
{
    public static class ExtensionsMethods
    {
        public static int ToInt32(this double value)
        {
            return Convert.ToInt32(value);
        }
    }
}
