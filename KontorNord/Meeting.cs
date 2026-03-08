using System;
using System.Collections.Generic;
using System.Text;

namespace KontorNord
{
    internal class Meeting
    {
        public int IsoYear;
        public int IsoWeek;
        public int IsoDay; // 1=Man ... 5=Fre

        // Forenklet møde-model der kun har helt tal mellem 8-17 som værdi
        public int StartHour;
        public int EndHour;

        public string Room = "";
        public string Participants = "";
        public string Note = "";

        public override string ToString()
        {
            return $"{TimeRangeText()}  Lokale: {Room}  Deltagere: {Participants}  Note: {Note}";
        }


        public string TimeRangeText() // metode der laver heltallene StartHour og EndHour om til et tekst-format der kan vises i cellen, fx 8 og 10 bliver til "08:00-10:00"
        {
            return $"{StartHour:00}:00-{EndHour:00}:00";
        }

    }
}
