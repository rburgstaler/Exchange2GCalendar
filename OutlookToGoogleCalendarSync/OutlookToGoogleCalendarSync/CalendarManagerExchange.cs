using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace OutlookToGoogleCalendarSync
{
    public class CalendarManagerExchange
    {

        private static bool CertificateValidationCallBack(
        object sender,
        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        System.Security.Cryptography.X509Certificates.X509Chain chain,
        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        public static List<CalendarEvent> GetAllEvents(String user, String pass)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();

            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            service.Credentials = new WebCredentials(user, pass);

            service.TraceEnabled = true;
            service.TraceFlags = TraceFlags.All;

            service.AutodiscoverUrl(user, RedirectionUrlValidationCallback);


            CalendarEvent cEvent = null;


            // Initialize values for the start and end times, and the number of appointments to retrieve.
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(60);
            const int NUM_APPTS = 500;

            // Initialize the calendar folder object with only the folder ID. 
            CalendarFolder calendar = CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());

            // Set the start and end time and number of appointments to retrieve.
            CalendarView cView = new CalendarView(startDate.AddDays(-30), endDate, NUM_APPTS);

            // Limit the properties returned to the appointment's subject, start time, and end time.
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.IsRecurring, AppointmentSchema.Id, AppointmentSchema.Location);

            // Retrieve a collection of appointments by using the calendar view.
            FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);

            Console.WriteLine("\nThe first " + NUM_APPTS + " appointments on your calendar from " + startDate.Date.ToShortDateString() +
                              " to " + endDate.Date.ToShortDateString() + " are: \n");

            foreach (Appointment a in appointments)
            {
                cEvent = new CalendarEvent(a.Id.ToString(), a.Start, a.End, a.Location, a.Subject, "Body not available");
                events.Add(cEvent);
            }
            /*
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
             */

            return events;
        }
    }
}
