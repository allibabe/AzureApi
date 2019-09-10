using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokGamesApi
{
    public class TokGamesRoomBasic
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = "lobby";

        [JsonProperty(PropertyName = "waiting_players")]
        public List<GamePlayer> WaitingPlayers { get; set; }


        //[JsonProperty(PropertyName = "playing_players")]
        //public List<string> PlayingPlayers { get; set; }
    }

    public class DayCounter
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = "";

        [JsonProperty(PropertyName = "messages")]
        public int Messages { get; set; }
    }
}
