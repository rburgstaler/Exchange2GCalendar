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
using LibEx2GCal;

namespace Ex2GCal
{
    public partial class Ex2GCalForm : Form
    {
        private void Msg(String AMsg, params Object[] list)
        {
            textBox1.AppendText(String.Format(AMsg, list) + Environment.NewLine);
        }
        
        public Ex2GCalForm()
        {
            InitializeComponent();
        }

        private CalendarService GetGoogleCalendarService()
        {
            IList<string> scopes = new List<string>();
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

        private void btSaveGoogle_Click(object sender, EventArgs e)
        {
            par.Save();
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
                ThreadMsg(calendarEvent.Summary + ". Start at: " + startDate);

                GetGoogleCalendarService().Events.Delete(tbCalendar.Text, calendarEvent.Id).Execute();
                ThreadMsg("Deleted " + calendarEvent.Id);

            }
        }

        private void btListAll_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            OperateOnEvents(false);
        }

        private void btDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to delete all items ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                textBox1.Text = "";
                Thread thd = new Thread(new ThreadStart(
                    delegate()
                    {
                        OperateOnEvents(true);
                    }));
                thd.Start();
            }
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

        SynchParams par = new SynchParams();
        private void PerformSynch()
        {
            Synch.PerformSynch(ThreadMsg, par);
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

        bool ignoreParam_TextChanged = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            par.Load();
            ignoreParam_TextChanged = true;
            try
            {
                tbClientID.Text = par.GoogleClientID;
                tbClientSecret.Text = par.GoogleClientSecret;
                tbCalendar.Text = par.GoogleCalendar;

                tbExchangeUserName.Text = par.ExchangeUserName;
                tbExchangePassword.Text = par.ExchangePassword;
                tbExchangeURL.Text = par.ExchangeURL;
            }
            finally
            {
                ignoreParam_TextChanged = false;
            }
        }

        private void Param_TextChanged(object sender, EventArgs e)
        {
            if (ignoreParam_TextChanged) return;
            par.GoogleClientID = tbClientID.Text;
            par.GoogleClientSecret = tbClientSecret.Text;
            par.GoogleCalendar = tbCalendar.Text;
            par.ExchangeURL = tbExchangeURL.Text;
            par.ExchangeUserName = tbExchangeUserName.Text;
            par.ExchangePassword = tbExchangePassword.Text; 
        }

    }
}
