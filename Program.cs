using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace hubspotApp
{
    public class Rootobject
    {
        public string name { get; set; }
        public int age { get; set; }
    }
    class Program
    {

        static async Task<string> APIcall(HttpMethod method, string uri, HubspotOutput r = null, Dictionary<string, string> RequestHeader = null)
        {
            HttpClient client = new HttpClient();

            // Add a new Request Message
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);

            // Add our custom headers
            if (RequestHeader != null)
            {
                foreach (var item in RequestHeader)
                {

                    requestMessage.Headers.Add(item.Key, item.Value);

                }
            }
            if (method == HttpMethod.Post)
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(requestMessage);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        static void Main(string[] args)
        {
            string getURI = "https://candidate.hubteam.com/candidateTest/v3/problem/dataset?userKey=c6ecf2ff937d313d57b670b147f4";
            string postURI = "https://candidate.hubteam.com/candidateTest/v3/problem/result?userKey=c6ecf2ff937d313d57b670b147f4";
            var inputFromAPI =// "{            \"partners\": [          {            \"firstName\": \"Darin\",            \"lastName\": \"Daignault\",            \"email\": \"ddaignault@hubspotpartners.com\",            \"country\": \"United States\",            \"availableDates\": [            \"2017-05-03\",            \"2017-05-06\"            ]          },          {            \"firstName\": \"Crystal\",            \"lastName\": \"Brenna\",            \"email\": \"cbrenna@hubspotpartners.com\",            \"country\": \"Ireland\",            \"availableDates\": [            \"2017-04-27\",            \"2017-04-29\",            \"2017-04-30\"            ]          },          {            \"firstName\": \"Janyce\",            \"lastName\": \"Gustison\",            \"email\": \"jgustison@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-29\",            \"2017-04-30\",            \"2017-05-01\"            ]          },          {            \"firstName\": \"Tifany\",            \"lastName\": \"Mozie\",            \"email\": \"tmozie@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-28\",            \"2017-04-29\",            \"2017-05-01\",            \"2017-05-04\"            ]          },          {            \"firstName\": \"Temple\",            \"lastName\": \"Affelt\",            \"email\": \"taffelt@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-28\",            \"2017-04-29\",            \"2017-05-02\",            \"2017-05-04\"            ]          },          {            \"firstName\": \"Robyn\",            \"lastName\": \"Yarwood\",            \"email\": \"ryarwood@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-29\",            \"2017-04-30\",            \"2017-05-02\",            \"2017-05-03\"            ]          },          {            \"firstName\": \"Shirlene\",            \"lastName\": \"Filipponi\",            \"email\": \"sfilipponi@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-30\",            \"2017-05-01\"            ]          },          {            \"firstName\": \"Oliver\",            \"lastName\": \"Majica\",            \"email\": \"omajica@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-28\",            \"2017-04-29\",            \"2017-05-01\",            \"2017-05-03\"            ]          },          {            \"firstName\": \"Wilber\",            \"lastName\": \"Zartman\",            \"email\": \"wzartman@hubspotpartners.com\",            \"country\": \"Spain\",            \"availableDates\": [            \"2017-04-29\",            \"2017-04-30\",            \"2017-05-02\",            \"2017-05-03\"            ]          },          {            \"firstName\": \"Eugena\",            \"lastName\": \"Auther\",            \"email\": \"eauther@hubspotpartners.com\",            \"country\": \"United States\",            \"availableDates\": [            \"2017-05-04\",            \"2017-05-09\"            ]          }            ]          }";
                APIcall(HttpMethod.Get, getURI).Result;
            Hubspotinput resultOfAPI = JsonConvert.DeserializeObject<Hubspotinput>(inputFromAPI);
            
            var apipost = APIcall(HttpMethod.Post, postURI, ProcessData(resultOfAPI)).Result;
        }
        static HubspotOutput ProcessData(Hubspotinput input)
        {
            HubspotOutput hubspotOutput = new HubspotOutput();
            hubspotOutput.countries = new List<Country>();
            Dictionary<string, SortedSet<DateTime>> CountryandDates = new Dictionary<string, SortedSet<DateTime>>();
            foreach(var partner in input.partners)
            {
                if(!CountryandDates.ContainsKey(partner.country))
                {
                    CountryandDates[partner.country] =new SortedSet<DateTime>();
                }
                foreach(var date in partner.availableDates)
                {
                    var availableDate= DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    if (!CountryandDates[partner.country].Contains(availableDate))
                    {
                        CountryandDates[partner.country].Add(availableDate);
                    }
                }
                
            }
            foreach(KeyValuePair< string, SortedSet<DateTime>> entry in CountryandDates)
            {
                List<Partner> partners = input.partners.Where(x => x.country ==entry.Key).ToList();
                List<Partner> partnersdatemax = new List<Partner>();
                string dateval1=null;
                foreach (var date in entry.Value)
                {
                    List<Partner> partnersdate1 = new List<Partner>();
                        //partners.Where(x => x.availableDates.Where(y => y.Equals(date.Date.ToString())).ToList().Count>0).ToList();
                    foreach (var partner in partners)
                    {
                        if (partner.availableDates.Contains(date.Date.ToString("yyyy-MM-dd"))&& partner.availableDates.Contains(date.AddDays(1).ToString("yyyy-MM-dd")))
                        {
                            partnersdate1.Add(partner);
                        }
                    }
                    if(partnersdatemax.Count< partnersdate1.Count)
                    {
                        partnersdatemax = partnersdate1;
                        dateval1 = date.Date.ToString("yyyy-MM-dd");
                    }

                }
                var country = new Country();
                country.attendees = new List<string>();
                if(partnersdatemax.Count==0)
                {
                    country.attendeeCount = 0;
                    country.startDate = null;
                }
                else
                {
                    country.attendeeCount = partnersdatemax.Count();
                    country.startDate = dateval1;
                    foreach(var partner in partnersdatemax)
                    {
                        country.attendees.Add(partner.email);
                    }
                }
                country.name = entry.Key;
                hubspotOutput.countries.Add(country);
            }
            return hubspotOutput;

        }
    }
}
