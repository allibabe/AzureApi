using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllisterFuncionsTrial.Models
{
     public class TokketUsers:BaseModels
    {
        
        [JsonRequired]
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; } = "user";

        [JsonIgnore]
        public string IdToken { get; set; }

        [JsonIgnore]
        public string StreamToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "user_photo")]
        public string UserPhoto { get; set; }

        [JsonProperty(PropertyName = "cover_photo")]
        public string CoverPhoto { get; set; }

        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }

        [JsonProperty(PropertyName = "website")]
        public string Website { get; set; }

        [JsonProperty(PropertyName = "birthday")]
        public DateTime Birthday { get; set; }

        [JsonProperty(PropertyName = "birthdate")]
        public string BirthDate { get; set; }

        [JsonProperty(PropertyName = "birth_year")]
        public long BirthYear { get; set; }

        [JsonProperty(PropertyName = "birth_month")]
        public long BirthMonth { get; set; }

        [JsonProperty(PropertyName = "birth_day")]
        public long BirthDay { get; set; }

        [JsonProperty(PropertyName = "joined")]
        public DateTime Joined { get; set; } = DateTime.Now;

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; } = null;

        [JsonProperty(PropertyName = "is_locked_out")]
        public bool IsLockedOut { get; set; } = false;

        #region Ads
        [JsonProperty("no_ads_tokblitz", NullValueHandling = NullValueHandling.Ignore)]
        public bool NoAdsTokBlitz { get; set; } = false;
        #endregion

        [JsonProperty("toks", NullValueHandling = NullValueHandling.Ignore)]
        public long? Toks { get; set; } = null;

        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public long? Points { get; set; } = null;

        [JsonProperty("coins", NullValueHandling = NullValueHandling.Ignore)]
        public long? Coins { get; set; } = null;

        //[JsonProperty("strikes", NullValueHandling = NullValueHandling.Ignore)]
        //public long? Strikes { get; set; } = null;

        [JsonProperty("sets", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sets { get; set; } = null;

        [JsonProperty("deleted_toks", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedToks { get; set; } = null;

        [JsonProperty("deleted_points", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedPoints { get; set; } = null;

        [JsonProperty("deleted_coins", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedCoins { get; set; } = null;

        [JsonProperty("deleted_strikes", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedStrikes { get; set; } = null;

        [JsonProperty("deleted_sets", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedSets { get; set; } = null;

        #region Tok Blitz
        [JsonProperty("tokblitz_strikes", NullValueHandling = NullValueHandling.Ignore)]
        public long? StrikesTokBlitz { get; set; } = null;

        [JsonProperty("tokblitz_deleted_strikes", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedStrikesTokBlitz { get; set; } = null;

        [JsonProperty("tokblitz_saved", NullValueHandling = NullValueHandling.Ignore)]
        public long? SavedTokBlitz { get; set; } = null;

        [JsonProperty("tokBlastSaved", NullValueHandling = NullValueHandling.Ignore)]
        public long? SavedTokBlast { get; set; } = null;


        #endregion



        #region Reactions
        [JsonProperty(PropertyName = "reactions", NullValueHandling = NullValueHandling.Ignore)]
        public long? Reactions { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_reactions", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedReactions { get; set; } = null;

        [JsonProperty(PropertyName = "likes", NullValueHandling = NullValueHandling.Ignore)]
        public long? Likes { get; set; } = null;

        [JsonProperty(PropertyName = "dislikes", NullValueHandling = NullValueHandling.Ignore)]
        public long? Dislikes { get; set; } = null;

        [JsonProperty(PropertyName = "accurates", NullValueHandling = NullValueHandling.Ignore)]
        public long? Accurates { get; set; } = null;

        [JsonProperty(PropertyName = "inaccurates", NullValueHandling = NullValueHandling.Ignore)]
        public long? Inaccurates { get; set; } = null;

        [JsonProperty(PropertyName = "comments", NullValueHandling = NullValueHandling.Ignore)]
        public long? Comments { get; set; } = null;

        [JsonProperty(PropertyName = "reports", NullValueHandling = NullValueHandling.Ignore)]
        public long? Reports { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_likes", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedLikes { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_dislikes", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedDislikes { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_accurates", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedAccurates { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_inaccurates", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedInaccurates { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_comments", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedComments { get; set; } = null;

        [JsonProperty(PropertyName = "followers", NullValueHandling = NullValueHandling.Ignore)]
        public long? Followers { get; set; } = null;

        [JsonProperty(PropertyName = "following", NullValueHandling = NullValueHandling.Ignore)]
        public long? Following { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_followers", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedFollowers { get; set; } = null;

        [JsonProperty(PropertyName = "deleted_following", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeletedFollowing { get; set; } = null;
        #endregion
   



    }
}
