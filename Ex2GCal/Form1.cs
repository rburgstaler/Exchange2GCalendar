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

namespace Ex2GCal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //' Calendar scopes which is initialized on the main method.

        static IList<string> scopes = new List<string>();
        //' Calendar service.

        static CalendarService service;
        private void button1_Click(object sender, EventArgs e)
        {
            // Add the calendar specific scope to the scopes list.
            scopes.Add(CalendarService.Scope.Calendar);

            // Display the header and initialize the sample.
            Console.WriteLine("Google.Apis.Calendar.v3 Sample");
            Console.WriteLine("==============================");

            UserCredential credential = default(UserCredential);
            using (FileStream stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, scopes, "user", CancellationToken.None, new FileDataStore("Calendar.VB.Sample")).Result;
            }

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

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>Displays all calendars.</summary>
        private static void DisplayList(IList<CalendarListEntry> list)
        {
            Console.WriteLine("Lists of calendars:");
            foreach (CalendarListEntry item in list)
            {
                Console.WriteLine(item.Summary + ". Location: " + item.Location + ", TimeZone: " + item.TimeZone);
            }
        }

        /// <summary>Displays the calendar's events.</summary>
        private static void DisplayFirstCalendarEvents(CalendarListEntry list)
        {
            Console.WriteLine(Environment.NewLine + "Maximum 5 first events from {0}:", list.Summary);
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

                Console.WriteLine(calendarEvent.Summary + ". Start at: " + startDate);
            }
        }
    }
}
