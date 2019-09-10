using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Security.Claims;
using AllisterFuncionsTrial.Models;
using AllisterFuncionsTrial.Services;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace AllisterFuncionsTrial
{
    public static class GroupFunctions
    {
        
         // connection
        [FunctionName("NegotiateGroup")]
        public static SignalRConnectionInfo Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version +  "/negotiate/"+"{id}")] HttpRequest req,string id,
        [SignalRConnectionInfo(HubName = "chat", UserId = "{id}")]SignalRConnectionInfo connectionInfo,
        ILogger log)
        {
            return connectionInfo;
        }



          // add the team to a group 
        [FunctionName("AddToGroup")]
        public static async Task<IActionResult> AddToGroup(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version +  "/{group}/add/{userId}")]HttpRequest req,
      string group,
      string userId,
      [SignalR(HubName = "chat")]IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {




            await signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = userId,
                    GroupName = group,
                    Action = GroupAction.Add
                });

            return new OkObjectResult(userId);
        }

          // remove the team to a group
        [FunctionName("RemoveToGroup")]
          public static async Task<IActionResult> RemoveToGroup(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version + "/{group}/remove/{userId}")]HttpRequest req,
          string group,
          string userId,
        [SignalR(HubName = "chat")]IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            await signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = userId,
                    GroupName = group,
                    Action = GroupAction.Remove
                });

            return new OkObjectResult(userId);
        }

        


         // send message to  particular group only
        [FunctionName("SendMessageToGroup")]
        public static async Task<IActionResult> SendMessageAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version +  "/{group}/send")]HttpRequest message,
        string group,
        [SignalR(HubName = "chat")]IAsyncCollector<SignalRMessage> signalRMessages)
        {

            string requestBody = await new StreamReader(message.Body).ReadToEndAsync();
            var message2 = JsonConvert.DeserializeObject<GameMessage2>(requestBody);
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                  // the message will be sent to the group with this name
                    GroupName = group,
                    Target = "newMessage",
                    Arguments = new[] { message2 }
                });


            return new OkResult();
        }
        private static FeedOptions _crosspartition = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
        // check if the user has a room available and create if none
        [FunctionName("GetRoom")]
        public static async Task<IActionResult> GetUserRoom([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/getroom")]HttpRequest req, ILogger log,
          ExecutionContext context)
        {
            try
            {               

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var getter = JsonConvert.DeserializeObject<TokBlitzRoom>(requestBody);

              
                //IDocumentQuery<TokBlitzRoom> query;
                //query = Constants.Client.CreateDocumentQuery<TokBlitzRoom>(
                //    UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CollectionId),
                //    _crosspartition)
                //    .Where(x => x.RoomName == getter.RoomName && x.Label == "tokblitz_room")
                //    .AsDocumentQuery();

                //if (query != null)
                //{
                //    return new NotFoundResult();

                //}



                    var item = await Api<TokBlitzRoom>.CreateItemAsync(getter, Constants.PkRequest(getter.Id));

                    return new OkResult();

            




                //var item = await Api<TokBlitzRoom>.GetItemAsync(getter.roomId, Constants.PkRequest(getter.roomId));

                //if (item == null)
                //    return new NotFoundResult();



            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }

        }




    }
}



public class TokBlitzRoom {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("user_id")]
    public string userId { get; set; }

    [JsonProperty("players")]
    public List<GamePlayer> Players { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("room_name")]
    public string RoomName { get; set; }


}

public class GameMessage2
{


    [JsonProperty("user_id")]
    public string user_id { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}
