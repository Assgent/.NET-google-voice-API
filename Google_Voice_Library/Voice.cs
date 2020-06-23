using System;
using System.Security;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Google_Voice_Library
{
    public sealed class Voice : IDisposable
    {
        private static readonly string VOICE_URL_SMS = "https://voice.google.com/u/0/messages";
        private static readonly string VOICE_URL_CALL = "https://voice.google.com/u/5/calls";
        private static readonly string LOGIN_URL = "https://accounts.google.com/signin/oauth/identifier?client_id=717762328687-iludtf96g1hinl76e4lc1b9a82g457nn.apps.googleusercontent.com&scope=profile%20email&redirect_uri=https%3A%2F%2Fstackauth.com%2Fauth%2Foauth2%2Fgoogle&state=%7B%22sid%22%3A1%2C%22st%22%3A%2259%3A3%3ABBC%2C16%3A167d1b1c2b78a437%2C10%3A1592880449%2C16%3A2768d00883f11498%2Cb9f2a310e70b2db0e958d0e6f3d60538a13e2a2c42558c483637ce63c6ef9686%22%2C%22cdl%22%3Anull%2C%22cid%22%3A%22717762328687-iludtf96g1hinl76e4lc1b9a82g457nn.apps.googleusercontent.com%22%2C%22k%22%3A%22Google%22%2C%22ses%22%3A%220897d76460724515b1f8b1f346a0e16b%22%7D&response_type=code&o2v=1&as=sfiR-q4x2F7Vuwx4lCZI1A&flowName=GeneralOAuthFlow";
                                            //We login into stack overflow to circumvent google automated login detection

        private readonly IWebDriver browser;

        public Voice(IWebDriver browserIn) 
        {
            browser = browserIn;
        }

        ~Voice()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (browser != null)
                browser.Close();
        }

        public bool NeedLogin()
        {
            browser.Url = LOGIN_URL;

            return browser.PageSource.IndexOf("sign in", StringComparison.OrdinalIgnoreCase) >= 0; 
        }

        public void Login(string emailIn, SecureString passwordIn) 
        {
            if (!NeedLogin())
                return;

            browser.Url = LOGIN_URL;

            using (LoginAttempt login = new LoginAttempt(emailIn, passwordIn, browser))
            {
                login.TryLogin();
            }
        }

        public void SendText(Text text)
        {
            SendTexts(new Text[] { text });
        }

        public void SendTexts(Text[] texts)
        {
            if (NeedLogin())
                throw new InvalidOperationException("This instance must be logged into Google Voice.");

            foreach (Text text in texts) 
            {
                browser.Url = VOICE_URL_SMS;

                browser.FindElement(By.XPath("//*[@gv-id='send-new-message']")).Click();

                var numberInputBox = browser.FindElement(By.XPath("//*[@type='search']"));
                numberInputBox.SendKeys('+' + text.Destination.CountryCode.ToString() + ' ');
                numberInputBox.SendKeys(text.Destination.PhoneNumber);

                numberInputBox.SendKeys(Keys.Enter);

                var messageInputBox = browser.FindElement(By.Id("input_2"));
                messageInputBox.SendKeys(text.Message);
                messageInputBox.SendKeys(Keys.Enter);

                WaitUntilMessageSent(Utilities.GetUnixTimestamp());
            }
        }

        private void WaitUntilMessageSent(ulong currentTimestamp) //Wait until message has finished sending
        {
            ulong timeout = currentTimestamp + 10;

            new WebDriverWait(browser, TimeSpan.FromMilliseconds(500));
            while (browser.PageSource.Contains("Sending..."))
                if (Utilities.GetUnixTimestamp() > timeout) //Checks if 10 seconds have passed
                    throw new TimeoutException("An attempt to send a message has timed out!");
        }

        public void MakeCall(Call call)
        {
            throw new NotImplementedException("This feature has not yet been implemented!"); //TODO: Finish
        }

        public void MakeCalls(Call[] calls)
        {
            if (NeedLogin())
                throw new InvalidOperationException("This instance must be logged into Google Voice.");

            throw new NotImplementedException("This feature has not yet been implemented!"); //TODO: Finish
        }
    }

    class LoginException : Exception
    {
        public LoginException(string reason) : base(reason)
        {

        }
    }
}

