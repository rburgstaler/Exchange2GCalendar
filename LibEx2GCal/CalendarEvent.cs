using System;

namespace LibEx2GCal
{
    /// <summary>
    /// A calender event item
    /// </summary>
    public class CalendarEvent : IEquatable<CalendarEvent>
    {
        private string body;
        private DateTime endDate;
        private string location;
        private DateTime startDate;
        private string subject;
        private string id;

        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarEvent()
        {
        }
        public CalendarEvent(string id, DateTime startDate, DateTime endDate, string location, string subject, string body)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Location = location;
            this.Subject = subject;
            this.Body = body;
            this.Id = id;
        }

        public String ToString()
        {
            return String.Format("{0} {1} {2} {3} {4} {5}", id, StartDate, EndDate, Location, Subject, Body);
        }

        public string Id
        {
            get { return id; }
            set 
            {
                id = NonNullTrimmed(value);
            }
        }

        public string Body
        {
            get { return body; }
            set { body = NonNullTrimmed(value); }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = NonNullTrimmed(value); }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        //Method that ensures there is a non-null trimmed string
        //Some of the systems we are saving to trim padding
        private String NonNullTrimmed(String AStr)
        {
            return (AStr == null) ? "" : AStr.Trim();
        }

        public string Subject
        {
            get { return subject; }
            set 
            { subject = NonNullTrimmed(value); }
        }

        /// <summary>
        /// Compare 2 event objects and return true if the id's are equal
        /// </summary>
        public bool Equals(CalendarEvent other)
        {
            return (other.Id == this.Id);
        }

        //Limit the max length of a string
        public string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Compare 2 event objects and return true if they differ in one or more attribute values
        /// </summary>
        public bool IsChanged(CalendarEvent other)
        {
            if (other.Equals(this))
            {


                return (
                    //Google has a max length for the body and everything beyond will get cut off.  Because of this we say they are equal if the first non truncated bytes are equal.
                    TruncateString(other.Body, CalendarGlobals.MaxGoogleCalendarDescriptionFieldLimit) != TruncateString(this.Body, CalendarGlobals.MaxGoogleCalendarDescriptionFieldLimit)  
                    ||
                    other.endDate != this.endDate
                    ||
                    other.location != this.Location
                    ||
                    other.startDate != this.startDate
                    ||
                    other.subject != this.Subject
                    );
            }
            else
            {
                throw new Exception("Cannot see if 2 events have changed if they do not share the same ID.");
            }
        }

    }
}