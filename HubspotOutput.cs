using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hubspotApp
{
    public class HubspotOutput
    {
        public List<Country> countries { get; set; }
    }

    public class Country
    {
        public int attendeeCount { get; set; }
        public List<string> attendees { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
    }

}
