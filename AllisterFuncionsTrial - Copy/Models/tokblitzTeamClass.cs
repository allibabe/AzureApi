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
        public string Id { get; set; }
         // team name 
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "";
        // contains the user (userlocalid)-tokblitzteam
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; } = "";

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; } = "team";
    
        [JsonProperty(PropertyName = "team_points")]
        public int TeamPoints { get; set; } = 0;

        [JsonProperty(PropertyName = "team_image")]
        public string TeamImage { get; set; } = "";

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; } = "";

        [JsonProperty(PropertyName = "wins")]
        public int Wins { get; set; } = 0;

        [JsonProperty(PropertyName = "losses")]
        public int Losses { get; set; } = 0;

        [JsonProperty(PropertyName = "games_played")]
        public int GamesPlayed { get; set; } = 0;

        [JsonProperty(PropertyName = "total_seconds")]
        public int TotalSeconds { get; set; } = 0;

        [JsonProperty(PropertyName = "strikes_used")]
        public int StrikesUsed { get; set; } = 0;

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = "";
    
        //[JsonIgnore]
        [JsonProperty(PropertyName = "team_rank")]
        public int TeamRank { get; set; } = 0;
            
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        //DataTime format
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        //Unix time format
        [JsonProperty(PropertyName = "_ts")]
        public int _Timestamp { get; set; }

        [JsonProperty(PropertyName = "pk")]
        public string PartitionKey { get; set; }



    }
}
