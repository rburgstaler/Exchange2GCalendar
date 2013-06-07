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
            this.startDate = startDate;
            this.endDate = endDate;
            this.location = location;
            this.subject = subject;
            this.body = body;
            this.id = id;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// Compare 2 event objects and return true if the id's are equal
        /// </summary>
        public bool Equals(CalendarEvent other)
        {
            return (other.Id == this.id);
        }

        /// <summary>
        /// Compare 2 event objects and return true if they differ in one or more attribute values
        /// </summary>
        public bool IsChanged(CalendarEvent other)
        {
            if (other.Equals(this))
            {
                string testBody = this.body;
                string testLocation = this.location;
                string testSubject = this.subject;

                if (testBody == null)
                    testBody = "";

                if (testLocation == null)
                    testLocation = "";

                if (testSubject == null)
                    testSubject = "";

                return (
                    other.body.Replace("\r", "").Trim('\n').Trim() != testBody.Replace("\r", "").Trim('\n').Trim()
                    ||
                    other.endDate != this.endDate
                    ||
                    other.location != testLocation
                    ||
                    other.startDate != this.startDate
                    ||
                    other.subject != testSubject
                    );
            }
            else
            {
                throw new Exception("Cannot see if 2 events have changed if they do not share the same ID.");
            }
        }

    }
}