using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllisterFuncionsTrial.Models
{
    // this is the class that will hold the team data of the users
    public class tokblitzTeamClass
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "team_name")]
        public string teamname { get; set; } = "";

        [JsonProperty(PropertyName = "owned_by")]
        public string owned_by { get; set; } = "";

        [JsonProperty(PropertyName = "team_points")]
        public int teampoints { get; set; } = 0;

        [JsonProperty(PropertyName = "team_image")]
        public string teamimage { get; set; } = "";

        [JsonProperty(PropertyName = "team_country")]
        public string country { get; set; } = "";

    }
}
