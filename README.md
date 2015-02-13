Introduction
============

A console program, Windows forms application, and library to sync your Exchange Server to a specified Google calendar. The sync happens one way.

Prerequisites
=============

The following packages are requirements to compile but Visual Studio 2013 integrated Nuget should handle getting these for you :

* Calendar API Client Library for .NET v3: Use the following NuGet command to get this: Install-Package Google.Apis.Calendar.v3
* Microsoft Exchange Web Services Managed API 2.1: http://www.microsoft.com/en-us/download/details.aspx?id=42022: Use the following NuGet command to get this: Install-Package Microsoft.Exchange.WebServices

Usage
=====

Run Ex2GCal.exe:
1.) Enter in the Exchange username, password, and url
2.) Enter in the Google Client ID, and Client Secret
    The following info was found @: http://samples.google-api-dotnet-client.googlecode.com/hg/Calendar.VB.ConsoleApp/README.html
2.a.) Visit the Google APIs console (https://code.google.com/apis/console/)
2.b.) If this is your first time, click "Create project..."
2.c.) Otherwise, click on the drop down under the "Google APIs" logo at the top left, and click "Create..." under "Other projects"
2.d.) Click on "API Access", and then on "Create an OAuth 2.0 Client ID...".
2.e.) Enter a product name and click "Next".
2.f.) Select "Installed application" and click "Create client ID".
2.g.) In the newly created "Client ID for installed applications", copy the client ID and client secrets into the AdSenseSample.cs file.
2.h.) Activate the Calendar API for your project.
3.) Enter in the Google Calendar
3.a.) This can be found by clicking the "..." button next to the Calendar text box once the proper Client ID and Client Secret have been entered
