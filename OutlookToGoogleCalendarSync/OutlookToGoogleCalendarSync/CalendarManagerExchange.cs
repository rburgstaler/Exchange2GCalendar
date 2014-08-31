using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace OutlookToGoogleCalendarSync
{

    class TraceListener : ITraceListener
    {
        private void AddTraceLine(String AMsg)
        {
            Console.WriteLine(AMsg);

        }

        public void Trace(string traceType, string traceMessage)
        {
            AddTraceLine("================== " + traceType + " ==================");
            AddTraceLine(traceMessage);
        }
    }

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

        public static List<CalendarEvent> GetAllEvents(String exchangeurl, String user, String pass)
        {
            List<CalendarEvent> events = new List<CalendarEvent>();

            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            service.Credentials = new WebCredentials(user, pass);

            service.TraceEnabled = true;
            service.TraceFlags = TraceFlags.All;
            service.TraceListener = new TraceListener();


            //When attempting to connect to EWS, it is much quicker to just use the url directly
            //rather than using the auto-resolve.  So we will first attempt the url, if that works
            //then we are golden otherwise we will go for the auto-resolve.
            CalendarFolder calendar = null;
            if (exchangeurl != "")
            {
                service.Url = new Uri(exchangeurl);

                try
                {
                    // Try to initialize the calendar folder object with only the folder ID. 
                    calendar = CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());
                }
                catch (Exception exp)
                {
                    service.TraceListener.Trace("ExchangeUrlBindFail", exp.Message);
                }
            }

            //calendar will still be null if we still have not bound correctly
            if (calendar == null)
            {
                service.AutodiscoverUrl(user, RedirectionUrlValidationCallback);
                // Try to initialize the calendar folder object with only the folder ID. 
                calendar = CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());
            }

            CalendarEvent cEvent = null;


            // Initialize values for the start and end times, and the number of appointments to retrieve.
            const int NUM_APPTS = 1000;

            // Set the start and end time and number of appointments to retrieve.
            CalendarView cView = new CalendarView(CalendarGlobals.StartDate, CalendarGlobals.EndDate, NUM_APPTS);

            // Limit the properties returned to the appointment's subject, start time, and end time. (Body cannot be loaded with FindAppointments, we need to do that later)
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.IsRecurring, AppointmentSchema.Id, AppointmentSchema.Location);
            cView.PropertySet.RequestedBodyType = BodyType.Text;

            // Retrieve a collection of appointments by using the calendar view.
            FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);

            Console.WriteLine("\nThe first " + NUM_APPTS + " appointments on your calendar from " + CalendarGlobals.StartDate.Date.ToShortDateString() +
                              " to " + CalendarGlobals.EndDate.Date.ToShortDateString() + " are: \n");

            //Now that we have found the appointments that we want, we will be able to load the body
            cView.PropertySet.Add(AppointmentSchema.Body);
            service.LoadPropertiesForItems(appointments, cView.PropertySet);
            foreach (Appointment a in appointments)
            {
                cEvent = new CalendarEvent(a.Id.ToString(), a.Start, a.End, a.Location, a.Subject, a.Body);
                events.Add(cEvent);
            }
         
            return events;
        }
    }
}
