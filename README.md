# Etsy OAuth1.0 Authentication

EtsyOAuth1Authentication is a simple library which allows you to easily perform the first OAuth Authentication step in Etsys current API (v2)

[![Nuget](https://img.shields.io/nuget/v/EtsyOAuth1Authentication.svg)](https://www.nuget.org/packages/EtsyOAuth1Authentication/)

## Features
- Built on the .Net Standard library for use in old and new .Net
- Can be added as a service in the DI framework in .NET Core
- Simple and easy to use

## How to use

### 1. Setup an Etsy account at etsy.com
Got to https://www.etsy.com and create and account.

### 2. Register for a developer account
Whilst logged in with your etsy account, browse to https://www.etsy.com/developers/
Press the 'Register as a developer' button on the left

### 3. Create your App
Press the 'Create a New App' button
Configure you app to meet your current needs (this can be changed afterwards)

### 4. Take note of your KEYSTRING and SHAREDSECRET
Once the app has been created you will be given a KEYSTRING and SHAREDSECRET
You use these  for authenticating your app against Etsy's OAuth service and to recieve temporary tokens and a URL to call to ask the for permission to add your app to the users Etsy account.

### 5. Add this library to your solution and reference it in your project
Install the package from NuGet
```
dotnet add package EtsyOAuth1Authentication --version 1.0.0
```

### 6. Add the service (.Net Core Console App example)
```c#
var serviceProvider = new ServiceCollection()
                .AddEtsyOAuthScoped("KEYSTRING", "SHAREDSECRET") // Scoped version
                .AddEtsyOAuthTransient("KEYSTRING", "SHAREDSECRET") // Transient version
                .AddEtsyOAuthSingleton("KEYSTRING", "SHAREDSECRET") // Singleton version
                .BuildServiceProvider();
```

### 7. Instantiate the service (.Net Core Console App example)
```c#
var etsyOauth = serviceProvider.GetService<EtsyOauth>();
```

### 8. Call Etsy OAuth endpoint to get the Confirm URL and temporary tokens
```c#
string tempAccessToken = "";
string tempAccessSecret = "";
string confirmUrl = etsyOauth.GetConfirmUrlWithTempTokens(
                out tempAccessToken,
                out tempAccessSecret,
                new List<ScopesEnum> { ScopesEnum.listings_r, ScopesEnum.transactions_r },
                "CALLBACKURL(OPTIONAL)");
```
Scopes are documented on the Etsy developer portal
https://www.etsy.com/developers/documentation/getting_started/oauth#section_permission_scopes

### 9. Redirect your app to the Confirm URL
Send your App to the confirm URL (OPTIONAL: pass a callback url) and you will be asked to verify the apps access to the etsy store of the current logged in user, or it will ask the user to login.
Once access has been verified, the user will be given a Verification code on screen. Or if you passed a callback URL then Etsy will send that verification code to your callback URL for you to store for the next step

### 10. Get the permenant tokens to use against Etsy API endpoints
Now that you should have the following data
- TempAccessToken
- TempSecretToken
- VerficationCode

With these, you can call the following method which will give you the permenant access Token and Secret
```c#
string permAccessToken = "";
string permAccessSecret = "";
oAuth.ObtainTokenCredentials(tempAccessToken, 
                             tempAccessSecret,
                             verifyCode,
                             out permAccessToken,
                             out permAccessSecret);
```
Store these tokens in a secure location as they are the keys to your or your users etsy data (Scopes dependant of course)
