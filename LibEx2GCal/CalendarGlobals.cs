using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibEx2GCal
{
    public class CalendarGlobals
    {
        public static int MaxGoogleEntriesToReturn = 2400;  //Actual Google max of v3 interface is 2500
        public static DateTime StartDate = DateTime.Now.AddMonths(-6);
        public static DateTime EndDate = DateTime.Now.AddMonths(6);
    }
}
