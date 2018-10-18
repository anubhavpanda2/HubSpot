using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hubspotApp
{
    public class Hubspotinput
    {
        public Partner[] partners { get; set; }
    }

    public class Partner
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string[] availableDates { get; set; }
    }

}
