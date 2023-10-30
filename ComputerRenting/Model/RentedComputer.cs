using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRenting.Model
{
    public class RentedComputer
    {
        public RentedComputer() 
        {
            //this.color = (this.status == "Reserveret") ? Color.FromRgb(0,255,255) : Color.FromRgb(255,0,0);
        }
        [JsonProperty("udlånID")]
        public int rentId { get; set; }
        [JsonProperty("computerID")]
        public int id { get; set; }
        [JsonProperty("registreringsNummer")]
        public string register { get; set; }
        [JsonProperty("fabrikatNavn")]
        public string computer { get; set; }
        [JsonProperty("modelNavn")]
        public string model { get; set; }
        [JsonProperty("udlånsdato")]
        public string udlåningsdato { get; set; }
        [JsonProperty("udløbsdato")]
        public string udløbsdato { get; set; }
        [JsonProperty("musType")]
        public string mus { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        public Color color { get; set; }
    }
}
