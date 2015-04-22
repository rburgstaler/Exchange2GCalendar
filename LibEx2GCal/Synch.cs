using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibEx2GCal
{
    public delegate void SynchMessage(String Msg, params Object[] list);

    public class SynchParams
    {
        public SynchParams()
        {
            GoogleClientID = "";
            GoogleClientSecret = "";
            GoogleCalendar = "";
            ExchangeURL = "";
            ExchangeUserName = "";
            ExchangePassword = "";
        }
        public String GoogleClientID { get; set; }
        public String GoogleClientSecret { get; set; }
        public String GoogleCalendar { get; set; }
        public String ExchangeURL { get; set; }
        public String ExchangeUserName { get; set; }
        public String ExchangePassword { get; set; }

        static String DefaultConfigPath = "Ex2GCal.json";

        public void Load()
        {
            if (File.Exists(DefaultConfigPath))
            {
                JObject jsObj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(DefaultConfigPath));
                GoogleClientID = (string)(jsObj["client_id"] ?? "");
                GoogleClientSecret = (string)(jsObj["client_secret"] ?? "");
                GoogleCalendar = (string)(jsObj["calendar"] ?? "");

                ExchangeUserName = (string)(jsObj["ExchangeUserName"] ?? "");
                ExchangePassword = (string)(jsObj["ExchangePassword"] ?? "");
                ExchangeURL = (string)(jsObj["ExchangeURL"] ?? "");
            }
        }

        public void Save()
        {
            JObject jsObj = new JObject();
            jsObj["client_id"] = GoogleClientID;
            jsObj["client_secret"] = GoogleClientSecret;
            jsObj["calendar"] = GoogleCalendar;
            jsObj["ExchangeUserName"] = ExchangeUserName;
            jsObj["ExchangePassword"] = ExchangePassword;
            jsObj["ExchangeURL"] = ExchangeURL;

            String ser = JsonConvert.SerializeObject(jsObj, Formatting.Indented);
            File.WriteAllText(DefaultConfigPath, ser);
        }
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
                    msgCallback("Creating event \"{0}\", {1]", cEvent.Subject, cEvent.StartDate);
                    gManager.CreateEvent(cEvent);
                }
                else if (cEvent.IsChanged(searchEvent))
                {
                    // -> update event
                    msgCallback("Update event \"{0}\", {1} ", cEvent.Subject, cEvent.StartDate);
                    gManager.UpdateEvent(cEvent);
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
