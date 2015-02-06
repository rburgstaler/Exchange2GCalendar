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
        //' Calendar scopes which is initialized on the main method.

        private IList<string> scopes = new List<string>();
        //' Calendar service.

        private CalendarService service;
        private void button1_Click(object sender, EventArgs e)
        {
            // Add the calendar specific scope to the scopes list.
            scopes.Add(CalendarService.Scope.Calendar);

            // Display the header and initialize the sample.
            Msg("Google.Apis.Calendar.v3 Sample");
            Msg("==============================");

            UserCredential credential = null;

            ClientSecrets sec = new ClientSecrets();
            sec.ClientId = tbClientID.Text;
            sec.ClientSecret = tbClientSecret.Text;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(sec, scopes, "user", CancellationToken.None, new FileDataStore("Calendar.VB.Sample")).Result;

            // Create the calendar service using an initializer instance
            BaseClientService.Initializer initializer = new BaseClientService.Initializer();
            initializer.HttpClientInitializer = credential;
            initializer.ApplicationName = "VB.NET Calendar Sample";
            service = new CalendarService(initializer);

            // Fetch the list of calendar list
            IList<CalendarListEntry> list = service.CalendarList.List().Execute().Items;

            // Display all calendars
            DisplayList(list);
            foreach (CalendarListEntry calendar in list)
            {
                // Display calendar's events
                DisplayFirstCalendarEvents(calendar);
            }

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
        private void DisplayFirstCalendarEvents(CalendarListEntry list)
        {
            Msg(Environment.NewLine + String.Format("Maximum 5 first events from {0}:", list.Summary));
            EventsResource.ListRequest requeust = service.Events.List(list.Id);
            // Set MaxResults and TimeMin with sample values
            requeust.MaxResults = 5;
            requeust.TimeMin = new DateTime(2014, 10, 1, 20, 0, 0);
            // Fetch the list of events
            foreach (Google.Apis.Calendar.v3.Data.Event calendarEvent in requeust.Execute().Items)
            {
                string startDate = "Unspecified";
                if (((calendarEvent.Start != null)))
                {
                    if (((calendarEvent.Start.Date != null)))
                    {
                        startDate = calendarEvent.Start.Date.ToString();
                    }
                }

                Msg(calendarEvent.Summary + ". Start at: " + startDate);
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
    }
}
