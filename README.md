Introduction
============

A console program to sync your outlook calendar to a specified Google calendar. The sync happens one way.

Prerequisites
=============

Installing the following is a must to get the program to run:

* Google Data API for .NET: https://code.google.com/p/google-gdata/downloads/list
* The Microsoft Office 2010 Primary Interop Assemblies (PIA): https://www.microsoft.com/en-us/download/details.aspx?id=3508

If you use another version of office, you should install the appropriate PIA for that version and change the assembly reference.

Screenshots
============

A few screenshots of the program in action.

*Main view*  
![Before](https://raw.github.com/jorisv83/Outlook-To-Google-Calendar-Sync/master/Screenshots/running.png "Before run")

Usage
=====

In the app.config file you need to add your gmail username, password and the calendar to push the changes to.

How to find the calendar?
-------------------------
Create a calendar in google and find the calendar ID. To find the ID go to the calendar properties and find the XML button in the section “address for this calendar”.
The ID you are looking for is something like this: 123456789ABCDEFG%40group.calendar.google.com
