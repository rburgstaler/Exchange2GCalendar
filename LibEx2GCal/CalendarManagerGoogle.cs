using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LibEx2GCal
{
    /// <summary>
    /// Class to manage all google calendar requests
    /// </summary>
    public class CalendarManagerGoogle
    {
        private const string syncExtendedParameterName = "SynchID";


        private CalendarService CalService;
        private String CalendarID = "";

        private CalendarService GetGoogleCalendarService(string ClientID, string ClientSecret, string CalendarID)
        {
            IList<string> scopes = new List<string>();
            // Add the calendar specific scope to the scopes list.
            scopes.Add(CalendarService.Scope.Calendar);


            UserCredential credential = null;

            ClientSecrets sec = new ClientSecrets();
            sec.ClientId = ClientID;
            sec.ClientSecret = ClientSecret;
            //Temporarily use the Application EXE directory for the FileDataStore
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(sec, scopes, "user", CancellationToken.None, new FileDataStore(@"D:\Debug\Google", true)).Result;

            // Create the calendar service using an initializer instance
            BaseClientService.Initializer initializer = new BaseClientService.Initializer();
            initializer.HttpClientInitializer = credential;
            initializer.ApplicationName = "Ex2GCal";
            return new CalendarService(initializer);
        }        
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarManagerGoogle(string aClientID, string aClientSecret, string aCalendarID)
        {
            CalService = GetGoogleCalendarService(aClientID, aClientSecret, aCalendarID);
            CalendarID = aCalendarID;
        }

        /// <summary>
        /// Creates a google calendar event
        /// </summary>
        public void CreateEvent(CalendarEvent cEvent)
        {
            Event entry = new Event();

            entry.Summary = cEvent.Subject;
            entry.Description = cEvent.Body;

            entry.ExtendedProperties = new Event.ExtendedPropertiesData();
            entry.ExtendedProperties.Private = new Dictionary<string, string>();
            entry.ExtendedProperties.Private[syncExtendedParameterName] = cEvent.Id;

            entry.Location = cEvent.Location;

            entry.Start = new EventDateTime();
            entry.End = new EventDateTime();

            entry.Start.DateTime = cEvent.StartDate;
            entry.End.DateTime = cEvent.EndDate;

            CalService.Events.Insert(entry, CalendarID).Execute();
        }

        /// <summary>
        /// Update a calendar event
        /// </summary>
        public void UpdateEvent(CalendarEvent cEvent)
        {
            EventsResource.ListRequest request = CalService.Events.List(CalendarID);

            // Set MaxResults and TimeMin with sample values
            request.PrivateExtendedProperty = syncExtendedParameterName + "=" + cEvent.Id;
            Events lst = request.Execute();

            if (lst.Items.Count == 1)
            {
                Event firstMatchEntry = lst.Items[0];
                firstMatchEntry.Description = cEvent.Body;
                firstMatchEntry.Summary = cEvent.Subject;

                firstMatchEntry.Location = cEvent.Location;

                firstMatchEntry.Start = new EventDateTime();
                firstMatchEntry.End = new EventDateTime();

                firstMatchEntry.Start.DateTime = cEvent.StartDate;
                firstMatchEntry.End.DateTime = cEvent.EndDate;

                CalService.Events.Update(firstMatchEntry, CalendarID, firstMatchEntry.Id).Execute();
            }
            else if (lst.Items.Count > 1)
            {
                throw new Exception("UpdateEvent: Found more then one event with the given id.");
            }
        }

        /// <summary>
        /// Check if a given entry already exists in the calendar
        /// </summary>
        public CalendarEvent GetEventFromSyncId(string id)
        {
            EventsResource.ListRequest request = CalService.Events.List(CalendarID);

            request.PrivateExtendedProperty = syncExtendedParameterName + "=" + id;
            Events lst = request.Execute();

            if (lst.Items.Count == 1)
            {
                Event firstMatchEntry = lst.Items[0];
                CalendarEvent cEvent = new CalendarEvent()
                {
                    Id = id,
                    Location = firstMatchEntry.Location ?? "",
                    Subject = firstMatchEntry.Summary ?? "",
                    Body = firstMatchEntry.Description ?? ""
                };
                if ((firstMatchEntry.Start != null) && (firstMatchEntry.Start.DateTime != null)) cEvent.StartDate = firstMatchEntry.Start.DateTime.Value;
                if ((firstMatchEntry.End != null) && (firstMatchEntry.End.DateTime != null)) cEvent.EndDate = firstMatchEntry.End.DateTime.Value;

                return cEvent;
            }
            else if (lst.Items.Count > 1)
            {
                throw new Exception("GetEventFromSyncId: Found more then one event with the given id.");
            }
            return null;

        }

        /// <summary>
        /// Delete a given event from the google calendar
        /// </summary>
        public void DeleteEvent(string id)
        {
            EventsResource.ListRequest request = CalService.Events.List(CalendarID);

            request.PrivateExtendedProperty = syncExtendedParameterName + "=" + id;
            Events lst = request.Execute();

            if (lst.Items.Count == 1)
            {
                CalService.Events.Delete(CalendarID, lst.Items[0].Id).Execute();
            }
            else if (lst.Items.Count > 1)
            {
                throw new Exception("DeleteEvent: Found more then one event with the given id.");
            }

        }

        /// <summary>
        /// Gets all the calendars the user has write access to
        /// </summary>
        public List<string> GetAllOwnedCalendars()
        {
            List<string> calendars = new List<string>();

            IList<CalendarListEntry> list = CalService.CalendarList.List().Execute().Items;
            foreach (CalendarListEntry item in list) calendars.Add(item.Id);
            return calendars;
        }

        /// <summary>
        /// Gets a list of all the calender events in the given calendar
        /// </summary>
        public List<CalendarEvent> GetAllEvents()
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            EventsResource.ListRequest request = CalService.Events.List(CalendarID);

            // Set MaxResults and TimeMin with sample values
            request.MaxResults = 2500; //2500 is currently the max
            request.TimeMin = CalendarGlobals.StartDate;
            request.TimeMax = CalendarGlobals.EndDate;
            Events lst = request.Execute();

            foreach (Event evt in lst.Items)
            {
                CalendarEvent cEvent = new CalendarEvent()
                {
                    Id = "", //Start out as blank
                    Location = evt.Location ?? "",
                    Subject = evt.Summary ?? "",
                    Body = evt.Description ?? ""
                };
                if ((evt.Start != null) && (evt.Start.DateTime != null)) cEvent.StartDate = evt.Start.DateTime.Value;
                if ((evt.End != null) && (evt.End.DateTime != null)) cEvent.EndDate = evt.End.DateTime.Value;

                String tmpId = "";
                if ((evt.ExtendedProperties != null) && (evt.ExtendedProperties.Private != null) && (evt.ExtendedProperties.Private.TryGetValue(syncExtendedParameterName, out tmpId))) cEvent.Id = tmpId;
                events.Add(cEvent);
            }
            return events;
        }
    }
}