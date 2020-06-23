using System;
using System.Security;

namespace Google_Voice_Library
{
    public class Utilities
    {
        public static SecureString ToSecureString(string str)
        {
            SecureString retStr = new SecureString();

            foreach (char chr in str)
            {
                retStr.AppendChar(chr);
            }

            return retStr;
        }

        internal static ulong GetUnixTimestamp()
        {
            return (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
