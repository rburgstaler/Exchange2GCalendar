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
    public partial class Form1 : Form
    {
        private void Msg(String AMsg)
        {
            textBox1.Text += AMsg + Environment.NewLine;
        }
        
        public Form1()
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
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(sec, scopes, "user", CancellationToken.None, new FileDataStore("Calendar.VB.Sample")).Result;

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

            /*
Event event = new Event();
event.setSummary("Appointment");
event.setLocation("Somewhere");

ArrayList<EventAttendee> attendees = new ArrayList<EventAttendee>();
attendees.add(new EventAttendee().setEmail("attendeeEmail"));
// ...
event.setAttendees(attendees);

Date startDate = new Date();
Date endDate = new Date(startDate.getTime() + 3600000);
DateTime start = new DateTime(startDate, TimeZone.getTimeZone("UTC"));
event.setStart(new EventDateTime().setDateTime(start));
DateTime end = new DateTime(endDate, TimeZone.getTimeZone("UTC"));
event.setEnd(new EventDateTime().setDateTime(end));
             */

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
            }
        }

        private void btSaveGoogle_Click(object sender, EventArgs e)
        {
            JObject jsObj = new JObject();
            jsObj["client_id"] = tbClientID.Text;
            jsObj["client_secret"] = tbClientSecret.Text;
            jsObj["calendar"] = tbCalendar.Text;
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
    }
}
