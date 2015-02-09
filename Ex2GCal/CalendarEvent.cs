using System;

namespace OutlookToGoogleCalendarSync
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
                //Remove invalid id characters
                //FYI: We must replace the invalid characters with unique strings (not "")
                //because it was experienced that the differene between two id's can
                //be as simple as 12abdef3+3133/ee vs. 12abdef3/3133/ee  See the difference???
                id = id.Replace("/", "_plus_");
                id = id.Replace("+", "_dash_");
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

        /// <summary>
        /// Compare 2 event objects and return true if they differ in one or more attribute values
        /// </summary>
        public bool IsChanged(CalendarEvent other)
        {
            if (other.Equals(this))
            {


                return (
                    other.body.Replace("\r", "").Trim('\n').Trim() != this.Body.Replace("\r", "").Trim('\n').Trim()
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