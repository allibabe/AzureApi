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
