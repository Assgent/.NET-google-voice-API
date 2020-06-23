using System;
using System.Text.RegularExpressions;

namespace Google_Voice_Library
{
    public sealed class Number
    {
        public readonly ushort CountryCode;
        public readonly string PhoneNumber;

        public Number(int codeIn, string numberIn)
        {
            PhoneNumber = Regex.Replace(numberIn, @"[^\d]", "");

            if (PhoneNumber.Length != 10)
                throw new ArgumentException("Improperly formatted phone number.");

            _ = (codeIn <= ushort.MaxValue && codeIn >= ushort.MinValue) ? CountryCode = Convert.ToUInt16(codeIn) : throw new ArgumentException("Country code exceeds storage limits.");
        }
    }

    public class Text
    {
        public readonly Number Destination;
        public readonly string Message;

        public Text(Number destinationIn, string messageIn)
        {
            Destination = destinationIn;
            Message = messageIn;
        }
    }

    public class Call
    {
        public readonly Number Destination;
        public readonly object Audio;

        public Call(Number destinationIn, object Audio)
        {
            throw new NotImplementedException("This feature has not yet been implemented!"); //TODO: Finish
        }
    }
}
