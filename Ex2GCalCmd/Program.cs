using LibEx2GCal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex2GCalCmd
{
    class Program
    {

        static void Main(string[] args)
        {
            SynchParams par = new SynchParams();
            par.Load();
            Synch.PerformSynch(Console.WriteLine, par);
        }
    }
}
