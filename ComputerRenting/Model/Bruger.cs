using Newtonsoft.Json;

namespace ComputerRenting.Model
{
    public class Bruger
    {
        [JsonProperty("brugerID")]
        public int brugerID { get; set; }

        [JsonProperty("navn")]
        public string navn { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("adresse")]
        public string adresse { get; set; }

        [JsonProperty("brugerGruppeNavn")]
        public string brugerGruppeNavn { get; set; }

        [JsonProperty("postNrByNavn")]
        public string postNrByNavn { get; set; }

        [JsonProperty("stamKlasseNavn")]
        public string stamKlasseNavn { get; set; }

        [JsonProperty("cprNummer")]
        public string cprNummer { get; set; }

        [JsonProperty("adgangskode")]
        public string adgangskode { get; set; }

        [JsonProperty("elevNummer")]
        public string elevNummer { get; set; }

        [JsonProperty("postNrID")]
        public int postNrID { get; set; }
    }
}