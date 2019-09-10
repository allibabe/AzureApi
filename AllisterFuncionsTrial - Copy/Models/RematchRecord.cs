using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllisterFuncionsTrial.Models
{
    public class RematchRecord
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "pk")]
        public string Pk { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [JsonProperty("label")]
        public string Label { get; set; } = "tokblitz_rematch";

        [JsonProperty("player1_id")]
        public string Player1Id { get; set; }

        [JsonProperty("player2_id")]
        public string Player2Id { get; set; }

        [JsonProperty("player1_connection_id")]
        public string Player1ConnectionId { get; set; }

        [JsonProperty("player2_connection_id")]
        public string Player2ConnectionId { get; set; }

        [JsonProperty("player1_rematch")]
        public bool Player1Rematch { get; set; } = false;

        [JsonProperty("player2_rematch")]
        public bool Player2Rematch { get; set; } = false;

        [JsonProperty("player1_has_made_decision")]
        public bool Player1HasMadeDecision { get; set; } = false;

        [JsonProperty("player2_has_made_decision")]
        public bool Player2HasMadeDecision { get; set; } = false;

        [JsonProperty("players")]
        public List<GamePlayer> Players { get; set; } = new List<GamePlayer>();

    }
}
