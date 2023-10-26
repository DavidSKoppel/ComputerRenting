using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRenting.Model
{
    public class FreeComputer
    {
        [JsonProperty("computerID")]
        public int id { get; set; }
        [JsonProperty("fabrikatNavn")]
        public string computer { get; set; }
        [JsonProperty("modelNavn")]
        public string model { get; set; }
        [JsonProperty("musType")]
        public string mus { get; set; }
        [JsonProperty("registreringsNummer")]
        public string register { get; set; }
    }
}
