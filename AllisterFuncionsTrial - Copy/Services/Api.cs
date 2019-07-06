using AllisterFuncionsTrial.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllisterFuncionsTrial.Services
{
    public static class Api<T> where T : class
    {
        static Api()
        {

            Constants.Client.OpenAsync();

        }

        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        public static async Task<Document> CreateItemAsync(T item, RequestOptions options)
        {
            return await Constants.Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CollectionId), item, options);
        }

        public static async Task<Document> CreateItemAsyncKnowledge(T item, RequestOptions options)
        {
         
            return await Constants.Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CollectionIdProd), item, options);
        }

        public static async Task<Document> CreateItemAsyncGames(T item, RequestOptions options)
        {

            return await Constants.Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CollectionId), item, options);
        }



        private static FeedOptions getalli = new FeedOptions { MaxItemCount = -1 };
                
         // get in games container
        public static async Task<T> GetItemAsync(string id, RequestOptions options)
        {
            try
            {
                Document document = await Constants.Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.CollectionId, id), options);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) { return null; }
                else { throw; }
            }
        }




        // get in games container
        public static async Task<T> GetItemAsyncSaved(string pk)
        {
            try
            {
                Microsoft.Azure.Documents.Document document = await Constants.Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.CollectionId, pk),null);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) { return null; }
                else { throw; }
            }
        }




        public static async Task<T> GetItemAsyncInKnowledge(string id, RequestOptions options)
        {
            try
            {
                Microsoft.Azure.Documents.Document document = await Constants.Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseIdProd, Constants.CollectionIdProd,id), options);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) { return null; }
                else { throw; }
            }
        }




        public static async Task<T> GetItemAsyncInUsers(string id, RequestOptions options)
        {
            try
            {                                                                                               
                Microsoft.Azure.Documents.Document document = await Constants.Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseIdProd, Constants.CollectionIdProdUsers, id), options);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) { return null; }
                else { throw; }
            }
        }




        ///  updates the coins or strikes
        public static async Task<Document> UpdateItemAsync(string id, T item, RequestOptions options)
        {
            return await Constants.Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.CollectionIdProd, id), item, options);
        }

        ///  updates the saved games  ("Games container")
        public static async Task<Document> UpdateItemAsyncSaveGames(string id, T item, RequestOptions options)
        {
            return await Constants.Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.CollectionId, id), item, options);
        }




         // this will delete in games container
        public static async Task DeleteItemAsync(string id, RequestOptions options)
        {
            await Constants.Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.CollectionId, id), options);
        }






    }
}
