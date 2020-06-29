# .NET-google-voice-API
An unofficial .NET API for Google Voice, written in C#

--------------------------------
Documentation: 
--------------------------------

new Voice(IWebDriver driver) - Creates a new instance of "Voice" which acts on the Selenium webdriver supplied by the user (e.x ChromeDriver)

  void - .Dispose() - Disposes the WebDriver used by the "Voice" instance
  
  bool - .NeedLogin() - Checks & returns if the "Voice" instance needs to log into Google
  
  void - .Login(string email, SecureString password) - Attempts to log the current "Voice" instance into Google (by logging into StackOverflow to circumvent Google's automated login detection)
    throws: LoginException
    
  void - .SendText(Text text) - Sends a single SMS message to the destination specified 
    throws: InvalidOperationException, SMSSendException
    
  void - .SendTexts(Text[] texts) - Sends multiple SMS messages to the destination specified
    throws: InvalidOperationException, SMSSendException
    
  void - .MakeCall(Call call) - Not implemented yet!
  
  void - .MakeCalls(Call[] calls) - Not implemented yet!
