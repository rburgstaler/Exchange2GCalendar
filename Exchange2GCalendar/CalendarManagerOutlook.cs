using System;
using System.Collections.Generic;

namespace OutlookToGoogleCalendarSync
{
    /// <summary>
    /// Class to manage all outlook calendar events
    /// </summary>
    public class CalendarManagerOutlook
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarManagerOutlook()
        {

        }

        /// <summary>
        /// Gets a list of all events in outlook
        /// </summary>
        public static List<CalendarEvent> GetAllEvents()
        {
            List<CalendarEvent> events = new List<CalendarEvent>();

            Microsoft.Office.Interop.Outlook.Application oApp = null;
            Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder CalendarFolder = null;
            Microsoft.Office.Interop.Outlook.Items outlookCalendarItems = null;

            oApp = new Microsoft.Office.Interop.Outlook.Application();
            mapiNamespace = oApp.GetNamespace("MAPI");
            CalendarFolder = mapiNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar); outlookCalendarItems = CalendarFolder.Items;
            outlookCalendarItems.IncludeRecurrences = true;

            CalendarEvent cEvent = null;

            foreach (Microsoft.Office.Interop.Outlook.AppointmentItem item in outlookCalendarItems)
            {
                cEvent = null;

                if (item.IsRecurring)
                {
                    Microsoft.Office.Interop.Outlook.RecurrencePattern rp = item.GetRecurrencePattern();
                    DateTime first = new DateTime(2008, 8, 31, item.Start.Hour, item.Start.Minute, 0);
                    DateTime last = new DateTime(2008, 10, 1);
                    Microsoft.Office.Interop.Outlook.AppointmentItem recur = null;

                    for (DateTime cur = first; cur <= last; cur = cur.AddDays(1))
                    {
                        try
                        {
                            recur = rp.GetOccurrence(cur);
                            cEvent = new CalendarEvent(recur.GlobalAppointmentID, recur.Start, recur.End, recur.Location, recur.Subject, recur.Body);
                        }
                        catch
                        { }
                    }
                }
                else
                {

                    cEvent = new CalendarEvent(item.GlobalAppointmentID, item.Start, item.End, item.Location, item.Subject, item.Body);
                }

                if (cEvent != null)
                    events.Add(cEvent);
            }

            return events;
        }
    }
}
