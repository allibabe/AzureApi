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

namespace AllisterFuncionsTrial
{
    public static class create_user_knowledge
    {
        // this function will create a user in the knowledge container after creation of account in firebase 
        // it only needs an id in the query string
        [FunctionName("create_user_knowledge")]
        public static async Task<IActionResult> createusercounter([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/" + "createusercounter/" +"{id}")]HttpRequest req, ILogger log,
          ExecutionContext context,string id)
        {
            try
            {
                 // if there is a new item to add in knowledeg (update the usercounter class or tokketuser) 
                UserCounter item = new UserCounter();
                item.Id = id + "-counter";
                item.PartitionKey = id + "-counter";
                item.Reactions = 0;
                item.Sets = 0;
                item.DeletedCoins = 0;
                item.TokblastSaved = 1;
                item.TokblitzSaved = 1;
                item.StrikesTokBlitz = 1;
                item.UserId = id;
                item.Reports = 0;
                item.Followers = 0;
                item.DeletedComments = 0;
                item.DeletedPoints = 0;
                item.Label = "usercounter";
                item.Likes = 0;
                item.Points = 0;
                item.Toks = 0;
                item.Dislikes = 0;
                item.tokblitzNumTeam = 1;
                item.IsRoomPurchasedTokBlitz = false;

                await Api<UserCounter>.CreateItemAsyncKnowledge(item, Constants.PkRequest(item.PartitionKey));
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        // this will update the room from false to true after buying in paypal
        [FunctionName("updateUserRoom")]
        public static async Task<IActionResult> updateUserRoom([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/" + "updateuserroom/" + "{id}")]HttpRequest req, ILogger log,
         ExecutionContext context, string id)
        {
            try
            {

                var item = await Api<UserCounter>.GetItemAsyncInKnowledge(id +"-counter",Constants.PkRequest(id + "-counter"));
                if (item == null)
                    return new BadRequestResult();


                item.IsRoomPurchasedTokBlitz = true;
                await Api<UserCounter>.UpdateItemAsync(id +"-counter",item,Constants.PkRequest(id + "-counter"));
                return new OkResult();

             
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }


                                             



    }
}
