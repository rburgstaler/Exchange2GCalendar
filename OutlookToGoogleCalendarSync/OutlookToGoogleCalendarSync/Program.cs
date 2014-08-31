using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OutlookToGoogleCalendarSync
{
    internal class Program
    {
        private static string user, pass, calendarId;
        private static string exchangeurl, exchangeuser, exchangepassword;

        private static void Main(string[] args)
        {
            ReadConfig();

            CalendarManagerGoogle gManager = new CalendarManagerGoogle(user, pass, calendarId);
            
            //List<CalendarEvent> events = CalendarManagerOutlook.GetAllEvents();
            List<CalendarEvent> eventsInExchange = CalendarManagerExchange.GetAllEvents(exchangeurl, exchangeuser, exchangepassword);
            Console.WriteLine("Found {0} events in the outlook calendar", eventsInExchange.Count);

            List<CalendarEvent> eventsInGoogle = gManager.GetAllEvents();

            //Error check to make sure we do not need to implement paging
            if (eventsInGoogle.Count == CalendarGlobals.MaxGoogleEntriesToReturn) throw new Exception("Google event feed paging needs to be implemented.");
            Console.WriteLine("Found {0} events in the google calendar", eventsInGoogle.Count);

            CalendarEvent searchEvent;

            foreach (CalendarEvent cEvent in eventsInExchange)
            {
                //searchEvent = gManager.GetEventFromSyncId(cEvent.Id);
                searchEvent = eventsInGoogle.Find(t => t.Id.Equals(cEvent.Id));

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

            }

            //Find all events that exist in Google that do not exist in Exchange
            foreach (CalendarEvent googleEvent in eventsInGoogle)
            {
                if (eventsInExchange.Exists(t => t.Id.Equals(googleEvent.Id))) continue;

                Console.WriteLine("Deleting event \"{0}\" ... ", googleEvent.Subject);
                gManager.DeleteEvent(googleEvent.Id);
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
            user = ConfigurationManager.AppSettings["email"];
            pass = ConfigurationManager.AppSettings["pass"];
            exchangeurl = ConfigurationManager.AppSettings["exchangeurl"];
            exchangeuser = ConfigurationManager.AppSettings["exchangeuser"];
            exchangepassword = ConfigurationManager.AppSettings["exchangepassword"];
            calendarId = ConfigurationManager.AppSettings["calendarId"];
        }
    }
}