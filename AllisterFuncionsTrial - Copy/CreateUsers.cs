using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AllisterFuncionsTrial.Models;
using AllisterFuncionsTrial.Services;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AllisterFuncionsTrial
{
    public static class CreateUsers
    {
        //[FunctionName("CreateUsers")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    return name != null
        //        ? (ActionResult)new OkObjectResult($"Hello, {name}")
        //        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        //}

        private const string resources = "Creaters";
        [FunctionName("CreateUser2")]
        public static async Task<IActionResult> Creaters([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/" + resources)]HttpRequest req, ILogger log,
            ExecutionContext context)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Users item = JsonConvert.DeserializeObject<Users>(requestBody);
              
                await Api<Users>.CreateItemAsync(item, Constants.PkRequest(item.pk));
                return new OkResult();

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        private const string resourcesGames = "inGames";

        //[FunctionName("ReadUser")]
        //public static async Task<IActionResult> ReadUser([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + resourcesGames + "/{id}")]HttpRequest req, ILogger log,
        // ExecutionContext context, string id)
        //{

        //    //try
        //    //{
        //    //    dynamic item = await Api<dynamic>.GetItemAsync(id, Constants.PkRequest(id));
        //    //    return new OkObjectResult(item);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    return new BadRequestObjectResult(e.Message);
        //    //}


        //}

        private const string resourcesGamesSaved = "SavedGames";

        [FunctionName("ReadUserSaved")]
        public static async Task<IActionResult> ReadUserSavedGames([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + "resourcesGamesSaved" + "/" + "{id}")]HttpRequest req, ILogger log, string id)
        {

            try
            {
                dynamic item = await Api<dynamic>.GetItemAsync(id, Constants.PkRequest(id));
                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }


        }







        private const string resource = "user";

        [FunctionName("UserGet")]
        public static async Task<IActionResult> getUser([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + resource + "/{id}")]HttpRequest req, ILogger log,
            ExecutionContext context, string id)
        {
            try
            {
                var item = await GetUserInUsers(id);

                if (item == null)
                    return new NotFoundResult();
                //if (!item.IsValidUser())
                //    return new BadRequestResult();

                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }


        private const string resource2 = "knowledge";
        [FunctionName("GetInknowledge")]
        public static async Task<IActionResult> UserGetInknowledge2([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + resource2 + "/{id}")]HttpRequest req, ILogger log,
           ExecutionContext context, string id)
        {
            try
            {
                var item = await GetUserKowledge(id);

                if (item == null)
                    return new NotFoundResult();
                //if (!item.IsValidUser())
                //    return new BadRequestResult();

                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }





        public static async Task<dynamic> GetUserKowledge(string id)
        {
            ////Get counter
            var counter = await Api<dynamic>.GetItemAsyncInKnowledge(id + "-counter", Constants.PkRequest(id + "-counter"));
            
            return counter;
        }


        public static async Task<dynamic> GetUserInUsers(string id)
        {
            dynamic item = null;
            RequestOptions options;
            options = Constants.PkRequest(id);
            item = await Api<dynamic>.GetItemAsyncInUsers(id, Constants.PkRequest(id));

          
            return item;
        }

        private const string resource3 = "CoinsStrikes";
        [FunctionName("CoinsAndStrikes")]
        public static async Task<IActionResult> CoinsStrikesUpdate([HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Version + "/" + resource3 + "/{id}")]HttpRequest req, ILogger log,
          ExecutionContext context, string id)
        {
            try
            {
             
                var item = await Api<dynamic>.GetItemAsyncInKnowledge(id + "-counter", Constants.PkRequest(id + "-counter"));
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var newItem = JsonConvert.DeserializeObject<dynamic>(requestBody);
                /// make sure to put value in coins (0) if the only thing that needs to be updated is the strikes
                if (newItem.coins == 0 || newItem.coins == null)
                {

                }
                else
                {

                    item.coins = newItem.coins;
                }


                item.tokblitz_strikes = newItem.tokblitz_strikes;
                await Api<dynamic>.UpdateItemAsync(item.id, item, Constants.PkRequest(item.pk));
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

            //        // only needed is the plain id and request body (ei {
            //    "id":"KDXLgt81yXUcJk9PIM6KMssWMwk1-counter",
            //    "tokblitz_saved":3
    
            //})


       private const string tokblitzSaved = "tokblitzSaved";
        [FunctionName("TokblitzSavedUpdate")]
        public static async Task<IActionResult> TokblitzSavedUpdate([HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Version + "/" + tokblitzSaved + "/{id}")]HttpRequest req, ILogger log,
          ExecutionContext context, string id)
        {
            try
            {

                var item = await Api<dynamic>.GetItemAsyncInKnowledge(id + "-counter", Constants.PkRequest(id + "-counter"));
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var newItem = JsonConvert.DeserializeObject<dynamic>(requestBody);
              
                
                item.TokblitzSaved = newItem.tokblitz_saved;
                await Api<dynamic>.UpdateItemAsync(item.id, item, Constants.PkRequest(item.pk));
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }










        // new save games API

        private const string SaveGames = "SaveGames";
        [FunctionName("saveGamesFunction")]
        public static async Task<IActionResult> saveGamesFunction([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/" + "SaveGames" + "/" +"{id}")]HttpRequest req, ILogger log,
          ExecutionContext context, string id)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                newSavedGames itemTosendFirst = JsonConvert.DeserializeObject<newSavedGames>(requestBody);
                newSavedGames item = await Api<newSavedGames>.GetItemAsync(id, Constants.PkRequest(id));
               
                if (item == null)
                {
                    newSavedGames itemTosend = JsonConvert.DeserializeObject<newSavedGames>(requestBody);
                    itemTosend.id = id;
                    itemTosend.fk = id;

                    await Api<newSavedGames>.CreateItemAsync(itemTosend, Constants.PkRequest(itemTosend.id));

                    var itemToUpdate = await Api<dynamic>.GetItemAsyncInKnowledge(itemTosend.User_id + "-counter", Constants.PkRequest(itemTosend.User_id + "-counter"));
                   // dynamic requestBodyFromItem = await new StreamReader(req.Body).ReadToEndAsync();

                  
                    if (itemTosendFirst.tokblitz_saved !=0) {
                        itemToUpdate.TokblitzSaved = itemTosendFirst.tokblitz_saved;
                    }

                    if (itemTosendFirst.tokBlastSaved != 0)
                    {
                        itemToUpdate.tokBlastSaved = itemTosendFirst.tokBlastSaved;
                    }


                    //itemToUpdate.TokblitzSaved = newItem.tokblitz_saved;


                    await Api<dynamic>.UpdateItemAsync(itemToUpdate.id, itemToUpdate, Constants.PkRequest(itemToUpdate.pk));
                    return new OkResult();
                    

                }
                else {
                    newSavedGames itemTosend = JsonConvert.DeserializeObject<newSavedGames>(requestBody);
                    string holder1 = item.Game_name.ToString();
                    string holder2 =itemTosend.Game_name.ToString();

                    item.level = itemTosend.level;
                    item.Toks = itemTosend.Toks;
                    item.Game_name = holder2;
                    await Api<newSavedGames>.UpdateItemAsyncSaveGames(id, item, Constants.PkRequest(id));
                    return new OkResult();
                    
                }

               
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }






        // new class for saved games 
        public class newSavedGames {
            public string id { get; set; }
            public string fk { get; set; }
            public string Game_name { get; set; }

            public List<int> level { get; set; }
            public List<string> Toks { get; set; }
            public string User_id { get; set; }
            public int tokBlastSaved { get; set; } = 0;
            public int tokblitz_saved { get; set; } = 0;
        }




        // temp class to update the coins and strikes
        public class CoinsStrikes
        {

            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "pk")]
            public string PK { get; set; }


            [JsonProperty(PropertyName = "coins")]
            public int Coins { get; set; }

            [JsonProperty(PropertyName = "tokblitz_strikes")]
            public int TokblitzStrikes { get; set; }


        }



        [FunctionName("UserGetMain")]
        public static async Task<IActionResult> UserGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + "UsersMain" + "/{id}")]HttpRequest req, ILogger log,
           ExecutionContext context, string id)
        {
            try
            {
                var item = await GetTokketUserById(id);

                if (item == null)
                    return new NotFoundResult();
                //if (!item.IsValidUser())
                //    return new BadRequestResult();

                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
        
        public static async Task<TokketUsers> GetTokketUserById(string id)
        {
            TokketUsers item = null;
            RequestOptions options;
            options = Constants.PkRequest(id);
            //Constants.DatabaseId = "TokketUsers"; Constants.CollectionId = "Users";

            item = await Api<dynamic>.GetItemAsyncInUsers(id, options);

            ////Get counter
           // Constants.CollectionId = "Knowledge";
            var counter = await Api<UserCounter>.GetItemAsyncInKnowledge(id + "-counter", Constants.PkRequest(id + "-counter"));
            if (counter != null)
            {
                item.SetCounts(counter);
            }

            return item;
        }


        // delete saved Games all new
        [ FunctionName("DeleteAllSaves")]
        public static async Task<IActionResult> DeleteAllSaveGames([HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.Version +"DeleteSaves" + "/" + "{id}")]HttpRequest req, ILogger log,
          ExecutionContext context, string id)
        {

            await Api<newSavedGames>.DeleteItemAsync(id, Constants.PkRequest(id));

            return new  OkResult();
        }




        public static TokketUsers SetCounts(this TokketUsers user, UserCounter counter)
        {
            if (counter == null)
            {
                user.Toks = 0;
                user.Points = 0;
                user.Coins = 0;
                user.Sets = 0;
                user.StrikesTokBlitz = 0;
                user.Reactions = 0;
                user.Likes = 0;
                user.Dislikes = 0;
                user.Accurates = 0;
                user.Inaccurates = 0;
                user.Comments = 0;
                user.Reports = 0;
                user.Followers = 0;
                user.Following = 0;
            }
            else
            {
                user.Toks = (counter?.Toks != null) ? (long)(counter?.Toks) : 0;
                user.Points = (counter?.Points != null) ? (long)(counter?.Points) : 0;
                user.Coins = (counter?.Coins != null) ? (long)(counter?.Coins) : 0;
                user.Sets = (counter?.Sets != null) ? (long)(counter?.Sets) : 0;
                user.StrikesTokBlitz = (counter?.Strikes != null) ? (long)(counter?.StrikesTokBlitz) : 0;
                user.Reactions = (counter?.Reactions != null) ? (long)(counter?.Reactions) : 0;
                user.Likes = (counter?.Likes != null) ? (long)(counter?.Likes) : 0;
                user.Dislikes = (counter?.Dislikes != null) ? (long)(counter?.Dislikes) : 0;
                user.Accurates = (counter?.Accurates != null) ? (long)(counter?.Accurates) : 0;
                user.Inaccurates = (counter?.Inaccurates != null) ? (long)(counter?.Inaccurates) : 0;
                user.Comments = (counter?.Comments != null) ? (long)(counter?.Comments) : 0;
                user.Reports = (counter?.Reports != null) ? (long)(counter?.Reports) : 0;
                user.Followers = (counter?.Followers != null) ? (long)(counter?.Followers) : 0;
                user.Following = (counter?.Following != null) ? (long)(counter?.Following) : 0;
                user.SavedTokBlitz = (counter?.TokblitzSaved != null) ? (long)(counter?.TokblitzSaved) : 0;
                user.SavedTokBlast = (counter?.TokblastSaved!=null)? (long)(counter?.TokblastSaved) : 0;

            }

            return user;
        }







    }


}

