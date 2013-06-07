using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OutlookToGoogleCalendarSync
{
    internal class Program
    {
        private static string user, pass, calendarId;

        private static void Main(string[] args)
        {
            ReadConfig();

            CalendarManagerGoogle gManager = new CalendarManagerGoogle(user, pass, calendarId);
            List<CalendarEvent> events = CalendarManagerOutlook.GetAllEvents();
            List<CalendarEvent> eventsInGoogle = gManager.GetAllEvents();
            List<CalendarEvent> eventsProcessed = new List<CalendarEvent>();

            Console.WriteLine("Found {0} events in the outlook calendar", events.Count);
            CalendarEvent searchEvent;

            foreach (CalendarEvent cEvent in events)
            {
                searchEvent = gManager.GetEventFromSyncId(cEvent.Id);

                if (searchEvent == null)
                {   // -> create a new event
                    Console.Write("Creating event \"{0}\" ... ", cEvent.Subject);
                    gManager.CreateEvent(cEvent);
                    Console.WriteLine("Created");
                }
                else if (cEvent.IsChanged(searchEvent))
                {
                    // -> update event
                    Console.Write("Update event \"{0}\" ... ", cEvent.Subject);
                    gManager.UpdateEvent(cEvent);
                    Console.WriteLine("Updated");
                }

                eventsProcessed.Add(cEvent);
            }

            List<CalendarEvent> eventsNotProcessed = eventsInGoogle.Except<CalendarEvent>(eventsProcessed).ToList<CalendarEvent>();
            if (eventsNotProcessed.Count > 0)
            {
                //TO DO: Fix Delete, only 25 items in google list and I should also ask outlook if item still exists...
                foreach (CalendarEvent cEvent in eventsNotProcessed)
                {
                    //gManager.DeleteEvent(cEvent.Id);
                }
            }

#if DEBUG
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
#endif
        }

        /// <summary>
        /// Reads the app.config and fill's in the variables
        /// </summary>
        private static void ReadConfig()
        {
            Console.WriteLine("Configuratie inlezen");

            user = ConfigurationManager.AppSettings["email"];
            pass = ConfigurationManager.AppSettings["pass"];
            calendarId = ConfigurationManager.AppSettings["calendarId"];
        }
    }
}