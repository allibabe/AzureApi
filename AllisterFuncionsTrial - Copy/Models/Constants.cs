using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllisterFuncionsTrial.Models
{
   public static class Constants
    {
        public static bool IsProd = false;
        private static readonly string endpointUrl = GetEnvironmentVariable("CosmosDBAccountEndpoint");
        private static readonly string authorizationKey = GetEnvironmentVariable("CosmosDBAccountKey");
        public static string DatabaseId = GetEnvironmentVariable("DatabaseId");
        public static string CollectionId = GetEnvironmentVariable("CollectionId");

        private static readonly string authorizationKeyProd = GetEnvironmentVariable("CosmosDBAccountKeyProd");
        public static string DatabaseIdProd = GetEnvironmentVariable("DatabaseIdProd");
        public static string CollectionIdProd = GetEnvironmentVariable("CollectionIdProd");


        public static string CollectionIdProdUsers = GetEnvironmentVariable("CollectionIdProdUsers");


        //public static FirebaseConfig FirebaseConfig = new FirebaseConfig(FirebaseAppKey);
        //public static FirebaseAuthProvider AuthProvider = new FirebaseAuthProvider(FirebaseConfig);
        //public static FirebaseApp FirebaseAppAdmin;

        public const int DAILY_MSGS_MAX = 18000;
        public const int FINISHED_FIRST_BONUS = 100;

        public const int TOKFEED_COUNT = 5;
        public const int FOLLOW_ITEM_LIMIT = 5;

        public const long REPLICATE_POINTS = 50;
        public const long REPLICATE_COINS = 1;
        public const long NONDETAILED_POINTS = 100;
        public const long NONDETAILED_COINS = 2;
        public const long DETAILED_POINTS = 200;
        public const long DETAILED_COINS = 4;

        public const long REACTIONS_PER_PARTITION = 4200;


        //public static StreamClient StreamClient = new StreamClient(GetEnvironmentVariable("StreamApiKeyDev"), GetEnvironmentVariable("StreamApiSecretDev"));
        //private static string FirebaseAppKey = GetEnvironmentVariable("FirebaseAppKey");
        //public static string StorageConnection = "DefaultEndpointsProtocol=https;AccountName=tokketblob;AccountKey=/rzeDfLzhfCn1xlSQwx2eHErbRVwmMQ/lTVHrmy8CRyUP/M5SyYM0gni3X7cITDLXDA2Cble2PPGlUh5zVjI8g==";

        public const string Version = "v1";
        public static DocumentClient Client { get; set; } = new DocumentClient(new Uri(endpointUrl), authorizationKey);
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }


        public static RequestOptions PkRequest(string key)
        {
            return new RequestOptions() { PartitionKey = new Microsoft.Azure.Documents.PartitionKey(key) };
        }

        public static FeedOptions PkFeed(string key)
        {
            return new FeedOptions() { PartitionKey = new Microsoft.Azure.Documents.PartitionKey(key), EnableCrossPartitionQuery = true };
        }
        

    }
}
