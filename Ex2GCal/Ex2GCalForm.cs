using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ex2GCal
{
    public partial class Ex2GCalForm : Form
    {
        private void Msg(String AMsg, params Object[] list)
        {
            textBox1.Text += String.Format(AMsg, list) + Environment.NewLine;
        }
        
        public Ex2GCalForm()
        {
            InitializeComponent();
        }

        private CalendarService GetGoogleCalendarService()
        {
            IList<string> scopes = new List<string>();
            textBox1.Text = "";
            // Add the calendar specific scope to the scopes list.
            scopes.Add(CalendarService.Scope.Calendar);


            UserCredential credential = null;

            ClientSecrets sec = new ClientSecrets();
            sec.ClientId = tbClientID.Text;
            sec.ClientSecret = tbClientSecret.Text;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(sec, scopes, "user", CancellationToken.None, new FileDataStore(Path.GetDirectoryName(Application.ExecutablePath), true)).Result;

            // Create the calendar service using an initializer instance
            BaseClientService.Initializer initializer = new BaseClientService.Initializer();
            initializer.HttpClientInitializer = credential;
            initializer.ApplicationName = "VB.NET Calendar Sample";
            return new CalendarService(initializer);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            CalendarService service = GetGoogleCalendarService();

            // Fetch the list of calendar list
            IList<CalendarListEntry> list = service.CalendarList.List().Execute().Items;

            // Display all calendars
            DisplayList(list);
            foreach (CalendarListEntry calendar in list)
            {
                // Display calendar's events
                DisplayFirstCalendarEvents(service, calendar);

                
            }

            Event entry = new Event();

            entry.Summary = "Event Summary";
            entry.Description = "This event description";
            //entry. = cEvent.Body;

            entry.ExtendedProperties = new Event.ExtendedPropertiesData();
            entry.ExtendedProperties.Private = new Dictionary<string, string>();
            entry.ExtendedProperties.Shared = new Dictionary<string, string>();
            entry.ExtendedProperties.Private["a"] = "1";
            entry.ExtendedProperties.Shared["b"] = "2";

            entry.Start = new EventDateTime();
            entry.End = new EventDateTime();

            entry.Start.DateTime = DateTime.Now;
            entry.End.DateTime = DateTime.Now;

            service.Events.Insert(entry, tbCalendar.Text).Execute();


            Msg("Press any key to continue...");
        }

        /// <summary>Displays all calendars.</summary>
        private void DisplayList(IList<CalendarListEntry> list)
        {
            Msg("Lists of calendars:");
            foreach (CalendarListEntry item in list)
            {
                Msg(item.Summary + ". Location: " + item.Location + ", TimeZone: " + item.TimeZone + ", ID: " + item.Id);
            }
        }

        /// <summary>Displays the calendar's events.</summary>
        private void DisplayFirstCalendarEvents(CalendarService service, CalendarListEntry list)
        {
            Msg(Environment.NewLine + String.Format("Maximum 5 first events from {0}:", list.Summary));
            EventsResource.ListRequest requeust = service.Events.List(list.Id);

            // Set MaxResults and TimeMin with sample values
            requeust.PrivateExtendedProperty = "a=1";
            requeust.MaxResults = 50;
            requeust.TimeMin = new DateTime(2015, 2, 6, 20, 0, 0);
            // Fetch the list of events
            foreach (Google.Apis.Calendar.v3.Data.Event calendarEvent in requeust.Execute().Items)
            {
                string startDate = "Unspecified";
                if (calendarEvent.Start != null) startDate = calendarEvent.Start.DateTime.ToString() ?? calendarEvent.Start.Date.ToString() ?? startDate;
                Msg(calendarEvent.Summary + ". Start at: " + startDate);

                //service.Events.Delete(tbCalendar.Text, calendarEvent.Id).Execute();
                //Msg("Deleted " + calendarEvent.Id);
            
            }
        }

        private String ConfigPath()
        {
            return "Ex2GCal.json";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (File.Exists(ConfigPath()))
            {
                JObject jsObj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(ConfigPath()));
                tbClientID.Text = (string)(jsObj["client_id"] ?? "");
                tbClientSecret.Text = (string)(jsObj["client_secret"] ?? "");
                tbCalendar.Text = (string)(jsObj["calendar"] ?? "");

                tbExchangeUserName.Text = (string)(jsObj["ExchangeUserName"] ?? "");
                tbExchangePassword.Text = (string)(jsObj["ExchangePassword"] ?? "");
                tbExchangeURL.Text = (string)(jsObj["ExchangeURL"] ?? "");
            }
        }

        private void btSaveGoogle_Click(object sender, EventArgs e)
        {
            JObject jsObj = new JObject();
            jsObj["client_id"] = tbClientID.Text;
            jsObj["client_secret"] = tbClientSecret.Text;
            jsObj["calendar"] = tbCalendar.Text;
            jsObj["ExchangeUserName"] = tbExchangeUserName.Text;
            jsObj["ExchangePassword"] = tbExchangePassword.Text;
            jsObj["ExchangeURL"] = tbExchangeURL.Text;

            String ser = JsonConvert.SerializeObject(jsObj, Formatting.Indented);
            File.WriteAllText(ConfigPath(), ser);
        }

        private void btFindCalendars_Click(object sender, EventArgs e)
        {
            IList<CalendarListEntry> list = GetGoogleCalendarService().CalendarList.List().Execute().Items;
            Msg("Lists of calendars:");
            foreach (CalendarListEntry item in list)
                Msg("ID: " + item.Id + " Summary: " + item.Summary + ". Location: " + item.Location + ", TimeZone: " + item.TimeZone);
        }

        private void OperateOnEvents(bool aDeleteAll)
        {
            EventsResource.ListRequest request = GetGoogleCalendarService().Events.List(tbCalendar.Text);
            //requeust.PrivateExtendedProperty = "a=1";
            request.MaxResults = 2500;
            //request.TimeMin = new DateTime(2014, 1, 1, 0, 0, 0);
            // Fetch the list of events
            foreach (Google.Apis.Calendar.v3.Data.Event calendarEvent in request.Execute().Items)
            {
                string startDate = "Unspecified";
                if (calendarEvent.Start != null) startDate = calendarEvent.Start.DateTime.ToString() ?? calendarEvent.Start.Date.ToString() ?? startDate;
                Msg(calendarEvent.Summary + ". Start at: " + startDate);

                //service.Events.Delete(tbCalendar.Text, calendarEvent.Id).Execute();
                //Msg("Deleted " + calendarEvent.Id);

            }
        }

        private void btListAll_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            OperateOnEvents(false);
        }

        private void btDeleteAll_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            Msg("Not yet implemented");
        }

        delegate void ThreadProcType();
        delegate void ThreadProcCaller(ThreadProcType AProc);
        private void ThreadProc(ThreadProcType AProc)
        {
            if (InvokeRequired)
            {
                ThreadProcCaller d = new ThreadProcCaller(ThreadProc);
                Invoke(d, new object[] { AProc });
            }
            else
            {
                AProc();
            }

        }


        private void ThreadMsg(String msg, params Object[] list)
        {
            ThreadProc(
                delegate()
                {
                    Msg(msg, list);
                });
        }

        private void PerformSynch()
        {

            CalendarManagerGoogle gManager = new CalendarManagerGoogle(tbClientID.Text, tbClientSecret.Text, tbCalendar.Text);

            //List<CalendarEvent> events = CalendarManagerOutlook.GetAllEvents();
            ThreadMsg("Getting all events from Outlook Calendar");
            List<CalendarEvent> eventsInExchange = CalendarManagerExchange.GetAllEvents(tbExchangeURL.Text, tbExchangeUserName.Text, tbExchangePassword.Text);
            ThreadMsg("Found {0} events in the Outlook calendar", eventsInExchange.Count);

            List<CalendarEvent> eventsInGoogle = gManager.GetAllEvents();

            //Error check to make sure we do not need to implement paging
            if (eventsInGoogle.Count == CalendarGlobals.MaxGoogleEntriesToReturn) throw new Exception("Google event feed paging needs to be implemented.");
            ThreadMsg("Found {0} events in the google calendar", eventsInGoogle.Count);

            CalendarEvent searchEvent;

            foreach (CalendarEvent cEvent in eventsInExchange)
            {
                //searchEvent = gManager.GetEventFromSyncId(cEvent.Id);
                searchEvent = eventsInGoogle.Find(t => t.Id.Equals(cEvent.Id));

                if (searchEvent == null)
                {   // -> create a new event
                    ThreadMsg("Creating event \"{0}\" ... ", cEvent.Subject);
                    gManager.CreateEvent(cEvent);
                    ThreadMsg("Created");
                }
                else if (cEvent.IsChanged(searchEvent))
                {
                    // -> update event
                    ThreadMsg("Update event \"{0}\" ... ", cEvent.Subject);
                    gManager.UpdateEvent(cEvent);
                    ThreadMsg("Updated");
                }

            }

            //Find all events that exist in Google that do not exist in Exchange
            List<CalendarEvent> deleteList = new List<CalendarEvent>();
            foreach (CalendarEvent googleEvent in eventsInGoogle)
            {
                if (eventsInExchange.Exists(t => t.Id.Equals(googleEvent.Id))) continue;
                deleteList.Add(googleEvent);

            }

            ThreadMsg("Found {0} events needing deletion in google", deleteList.Count);
            foreach (CalendarEvent googleEvent in deleteList)
            {
                ThreadMsg("Deleting event \"{0}\" {1} ... ", googleEvent.Subject, googleEvent.StartDate);
                gManager.DeleteEvent(googleEvent.Id);
            }

            //We are done so enable the synch button
            ThreadProc(
                delegate()
                {
                    btSynch.Enabled = true;
                });


        }

        private void btSynch_Click(object sender, EventArgs e)
        {
            Thread thd = new Thread(new ThreadStart(PerformSynch));
            btSynch.Enabled = false;
            thd.Start();
        }

    }
}
