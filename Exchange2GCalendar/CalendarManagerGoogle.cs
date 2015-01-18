using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using System;
using System.Collections.Generic;

namespace OutlookToGoogleCalendarSync
{
    /// <summary>
    /// Class to manage all google calendar requests
    /// </summary>
    public class CalendarManagerGoogle
    {
        private const string calendarsOwnedUrl = "https://www.google.com/calendar/feeds/default/owncalendars/full";
        private const string calendarUrl = "https://www.google.com/calendar/feeds/{0}/private/full";
        private const string serviceName = "jorisv83-googleOutlookSync-1";
        private const string syncExtendedParameterName = "JorisV83OutlookId";
        private string calendarId;
        private CalendarService calService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarManagerGoogle(string user, string password)
            : this(user, password, "default")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarManagerGoogle(string user, string password, string calendarId)
        {
            this.calService = new CalendarService(serviceName);
            this.calService.setUserCredentials(user, password);
            this.calendarId = calendarId;
        }

        /// <summary>
        /// Creates a google calendar event
        /// </summary>
        public void CreateEvent(CalendarEvent cEvent)
        {
            EventEntry entry = new EventEntry();

            entry.Title.Text = cEvent.Subject;
            entry.Content.Content = cEvent.Body;

            ExtendedProperty property = new ExtendedProperty();
            property.Name = syncExtendedParameterName;
            property.Value = cEvent.Id;
            entry.ExtensionElements.Add(property);

            Where eventLocation = new Where();
            eventLocation.ValueString = cEvent.Location;
            entry.Locations.Add(eventLocation);

            When eventTime = new When(cEvent.StartDate, cEvent.EndDate);
            entry.Times.Add(eventTime);

            Uri postUri = new Uri(string.Format(calendarUrl, this.calendarId));

            AtomEntry insertedEntry = this.calService.Insert(postUri, entry);
        }

        /// <summary>
        /// Update a calendar event
        /// </summary>
        public void UpdateEvent(CalendarEvent cEvent)
        {
            EventQuery query = new EventQuery(string.Format(calendarUrl, this.calendarId));
            query.ExtraParameters = string.Format("extq=[{1}:{0}]", cEvent.Id, syncExtendedParameterName);
            EventFeed myResultsFeed = this.calService.Query(query);
            if (myResultsFeed.Entries.Count == 1)
            {
                EventEntry firstMatchEntry = (EventEntry)myResultsFeed.Entries[0];
                firstMatchEntry.Content.Content = cEvent.Body;
                firstMatchEntry.Title.Text = cEvent.Subject;

                Where eventLocation = new Where();
                eventLocation.ValueString = cEvent.Location;
                firstMatchEntry.Locations.Clear();
                firstMatchEntry.Locations.Add(eventLocation);

                When eventTime = new When(cEvent.StartDate, cEvent.EndDate);
                firstMatchEntry.Times.Clear();
                firstMatchEntry.Times.Add(eventTime);

                this.calService.Update(firstMatchEntry);
            }
            else if (myResultsFeed.Entries.Count > 1)
            {
                throw new Exception("UpdateEvent: Found more then one event with the given id.");
            }
        }

        /// <summary>
        /// Check if a given entry already exists in the calendar
        /// </summary>
        public CalendarEvent GetEventFromSyncId(string id)
        {
            EventQuery query = new EventQuery(string.Format(calendarUrl, this.calendarId));
            //query.StartTime = cEvent.StartDate;
            //query.EndTime = cEvent.EndDate;
            //query.Query = cEvent.Id;
            query.ExtraParameters = string.Format("extq=[{1}:{0}]", id, syncExtendedParameterName);
            EventFeed myResultsFeed = this.calService.Query(query);
            if (myResultsFeed.Entries.Count == 1)
            {
                EventEntry firstMatchEntry = (EventEntry)myResultsFeed.Entries[0];
                CalendarEvent cEvent = new CalendarEvent(id,
                    firstMatchEntry.Times[0].StartTime,
                    firstMatchEntry.Times[0].EndTime,
                    firstMatchEntry.Locations[0].ValueString,
                    firstMatchEntry.Title.Text,
                    firstMatchEntry.Content.Content
                    );
                return cEvent;
            }
            else if (myResultsFeed.Entries.Count > 1)
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
            EventQuery query = new EventQuery(string.Format(calendarUrl, this.calendarId));
            query.ExtraParameters = string.Format("extq=[{1}:{0}]", id, syncExtendedParameterName);
            EventFeed myResultsFeed = this.calService.Query(query);
            if (myResultsFeed.Entries.Count == 1)
            {
                EventEntry firstMatchEntry = (EventEntry)myResultsFeed.Entries[0];
                firstMatchEntry.Delete();
            }
            else if (myResultsFeed.Entries.Count > 1)
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
            CalendarQuery query = new CalendarQuery();

            query.Uri = new Uri(calendarsOwnedUrl);
            CalendarFeed resultFeed = (CalendarFeed)this.calService.Query(query);

            foreach (CalendarEntry entry in resultFeed.Entries)
            {
                calendars.Add(entry.Title.Text);
            }

            return calendars;
        }

        /// <summary>
        /// Gets a list of all the calender events in the given calendar
        /// </summary>
        public List<CalendarEvent> GetAllEvents()
        {
            List<CalendarEvent> events = new List<CalendarEvent>();
            EventQuery query = new EventQuery(string.Format(calendarUrl, this.calendarId));
            query.StartTime = CalendarGlobals.StartDate;
            query.EndTime = CalendarGlobals.EndDate;
            query.NumberToRetrieve = CalendarGlobals.MaxGoogleEntriesToReturn; 
            EventFeed myResultsFeed = this.calService.Query(query);
            foreach (EventEntry entry in myResultsFeed.Entries)
            {
                string id = "";
                foreach (object obj in entry.ExtensionElements)
                {
                    if (obj is ExtendedProperty)
                    {
                        ExtendedProperty exProp = obj as ExtendedProperty;
                        if (exProp.Name == syncExtendedParameterName)
                        {
                            id = exProp.Value;
                            break;
                        }
                    }
                }

                events.Add(new CalendarEvent(id,
                    entry.Times[0].StartTime,
                    entry.Times[0].EndTime,
                    entry.Locations[0].ValueString,
                    entry.Title.Text,
                    entry.Content.Content
                    ));
            }
            return events;
        }
    }
}