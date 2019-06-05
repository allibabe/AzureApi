using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllisterFuncionsTrial.Models
{
   public class BaseModels
    {
        [JsonIgnore]
        string id = Guid.NewGuid().ToString("n");

        /// <summary>Id of the document</summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return Id1; }
            set
            {
                PartitionKey = value;
                Id1 = value;
            }
        }

        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [JsonIgnore]
        private string timestamp = DateTime.Now.ToString();

        //DataTime format
        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp
        {
            get { return timestamp; }
            set
            {
                timestamp = DateTime.Now.ToString();
                _Timestamp = DateTime.Now.ToString();
            }
        }

        //Unix time format
        [JsonProperty(PropertyName = "_ts")]
        public string _Timestamp { get; set; }

        /// <summary>Stores the type of database action: create, update, delete, duplicate</summary>
        [JsonProperty(PropertyName = "soft_marker", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SoftMarker { get; set; } = "none";

        /// <summary>Determines how the document is stored</summary>
        [JsonProperty(PropertyName = "pk")]
        public string PartitionKey { get; set; }

        public string _etag { get; set; }
        public string Id1 { get => id; set => id = value; }
    }
}
