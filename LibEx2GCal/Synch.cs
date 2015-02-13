using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibEx2GCal
{
    public delegate void SynchMessage(String Msg, params Object[] list);

    public class SynchParams
    {
        public String GoogleClientID { get; set; }
        public String GoogleClientSecret { get; set; }
        public String GoogleCalendar { get; set; }
        public String ExchangeURL { get; set; }
        public String ExchangeUserName { get; set; }
        public String ExchangePassword { get; set; }
    }

    public class Synch
    {

        static public void PerformSynch(SynchMessage msgCallback, SynchParams SynchParams)
        {

            CalendarManagerGoogle gManager = new CalendarManagerGoogle(SynchParams.GoogleClientID, SynchParams.GoogleClientSecret, SynchParams.GoogleCalendar);

            //List<CalendarEvent> events = CalendarManagerOutlook.GetAllEvents();
            msgCallback("Getting all events from Outlook Calendar");
            List<CalendarEvent> eventsInExchange = (new CalendarManagerExchange() { Status = new ExchangeStatus(msgCallback) }).GetAllEvents(SynchParams.ExchangeURL, SynchParams.ExchangeUserName, SynchParams.ExchangePassword);
            msgCallback("Found {0} events in the Outlook calendar", eventsInExchange.Count);

            List<CalendarEvent> eventsInGoogle = gManager.GetAllEvents();

            //Error check to make sure we do not need to implement paging
            if (eventsInGoogle.Count == CalendarGlobals.MaxGoogleEntriesToReturn) throw new Exception("Google event feed paging needs to be implemented.");
            msgCallback("Found {0} events in the google calendar", eventsInGoogle.Count);

            CalendarEvent searchEvent;

            foreach (CalendarEvent cEvent in eventsInExchange)
            {
                //searchEvent = gManager.GetEventFromSyncId(cEvent.Id);
                searchEvent = eventsInGoogle.Find(t => t.Id.Equals(cEvent.Id));

                if (searchEvent == null)
                {   // -> create a new event
                    msgCallback("Creating event \"{0}\" ... ", cEvent.Subject);
                    gManager.CreateEvent(cEvent);
                    msgCallback("Created");
                }
                else if (cEvent.IsChanged(searchEvent))
                {
                    // -> update event
                    msgCallback("Update event \"{0}\" ... ", cEvent.Subject);
                    gManager.UpdateEvent(cEvent);
                    msgCallback("Updated");
                }

            }

            //Find all events that exist in Google that do not exist in Exchange
            List<CalendarEvent> deleteList = new List<CalendarEvent>();
            foreach (CalendarEvent googleEvent in eventsInGoogle)
            {
                if (eventsInExchange.Exists(t => t.Id.Equals(googleEvent.Id))) continue;
                deleteList.Add(googleEvent);

            }

            msgCallback("Found {0} events needing deletion in google", deleteList.Count);
            foreach (CalendarEvent googleEvent in deleteList)
            {
                msgCallback("Deleting event \"{0}\" {1} ... ", googleEvent.Subject, googleEvent.StartDate);
                gManager.DeleteEvent(googleEvent.Id);
            }

        }
    }
}
