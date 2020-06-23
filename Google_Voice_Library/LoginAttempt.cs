using System;
using System.Security;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Google_Voice_Library
{
    public sealed class LoginAttempt : IDisposable
    {
        private readonly string email;
        private readonly SecureString password;
        private readonly IWebDriver browser;

        public LoginAttempt(string emailIn, SecureString passwordIn, IWebDriver browserIn)
        {
            email = emailIn;
            password = passwordIn;
            browser = browserIn;
        }

        ~LoginAttempt()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (password != null)
                password.Dispose();
        }

        public void TryLogin()
        {
            var emailInputBox = browser.FindElement(By.XPath("//*[@type='email']"));
            emailInputBox.SendKeys(email);

            emailInputBox.SendKeys(Keys.Enter);

            try { new WebDriverWait(browser, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@type='password']"))); }
            catch (WebDriverTimeoutException) { throw new LoginException("Email injection timed out. (Likely invalid email, or the machine's internet connection was interrupted.)"); }

            IWebElement passwordInputBox = browser.FindElement(By.XPath("//*[@type='password']"));

            passwordInputBox.SendKeys(new System.Net.NetworkCredential(string.Empty, password).Password);

            passwordInputBox.SendKeys(Keys.Enter);

            try { new WebDriverWait(browser, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='top-bar js-top-bar top-bar__network _fixed']"))); }
            catch (WebDriverTimeoutException) { throw new LoginException("Password injection timed out. (Likely invalid password, or the machine's internet connection was interrupted.)"); }
        }
    }
}
