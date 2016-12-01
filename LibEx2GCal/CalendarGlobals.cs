using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibEx2GCal
{
    public class CalendarGlobals
    {
        public static int MaxGoogleEntriesToReturn = 2400;  //Actual Google max of v3 interface is 2500
        //Google will cut off all text around or before 8192 characters in the description field (body)
        //I found a case where it cut it off at 9189 characters however the byte count was 81211 bytes.  (I am guessing because of the encoding)
        //Anyway we are going to play it safe and put it a fudge factor.
        public static int MaxGoogleCalendarDescriptionFieldLimit = 8192 - 92;  //92 character fudge factor  
        public static DateTime StartDate = DateTime.Now.AddMonths(-6);
        public static DateTime EndDate = DateTime.Now.AddMonths(6);
    }
}
