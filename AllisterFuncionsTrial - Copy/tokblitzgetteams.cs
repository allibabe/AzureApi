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
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using System.Reflection.Metadata;
using AllisterFuncionsTrial.Services;
using System.Collections;
using Newtonsoft.Json.Serialization;

namespace AllisterFuncionsTrial
{
    public static class tokblitzgetteams
    {
        private static FeedOptions _crosspartition = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
         // gets the team of a particular user
         // query string should have an id appended on it
        [FunctionName("tokblitzgetteams")]
        public static async Task<IActionResult> getter([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + "getteams/"+"{id}" )]HttpRequest req, ILogger log,
         ExecutionContext context , string id)
        {
            // here , we get first all te data in games cpntainer
            IQueryable<tokblitzTeamClass> Teams = Constants.Client.CreateDocumentQuery<tokblitzTeamClass>(
            UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId,Constants.CollectionId),_crosspartition);

            // append the {userlocalid}-tokblitzteam 
            string getId = id + "-tokblitzteam";
            // get all the team of a particular user
            IEnumerable<tokblitzTeamClass> get_team = Teams.Where(x => x.owned_by == getId);
            return new OkObjectResult(get_team);
        }

         // insert team created by the user
        [FunctionName("tokblitzinsertteam")]
        public static async Task<IActionResult> insertTeam([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/" + "insertteam") ]HttpRequest req, ILogger log,
         ExecutionContext context)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            tokblitzTeamClass getrequest = JsonConvert.DeserializeObject<tokblitzTeamClass>(requestBody);
            getrequest.owned_by = getrequest.id + "-tokblitzteam";
            getrequest.id = getrequest.id + Guid.NewGuid();

            var item = await Api<tokblitzTeamClass>.CreateItemAsyncGames(getrequest,Constants.PkRequest(getrequest.id));
            return new OkResult();
        }


        // deletes the team created by the user
        [FunctionName("deletetokblitzteam")]
        public static async Task<IActionResult> deletetokblitzteam([HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.Version + "/" + "deletetokblitzteam/" + "{id}")]HttpRequest req, ILogger log,
        ExecutionContext context, string id)
        {
            await Api<tokblitzTeamClass>.DeleteItemAsync(id ,Constants.PkRequest(id));

            return new OkResult();
        }


         //updates the team data of the user
        [FunctionName("updatetokblitzteam")]
        public static async Task<IActionResult> updatetokblitzteam([HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Version + "/" + "updatetokblitzteam/" + "{id}")]HttpRequest req, ILogger log,
        ExecutionContext context, string id)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            tokblitzTeamClass newItem = JsonConvert.DeserializeObject<tokblitzTeamClass>(requestBody);
             try {

                tokblitzTeamClass game = await Api<tokblitzTeamClass>.GetItemAsync(id, Constants.PkRequest(id));
                tokblitzTeamClass holder = new tokblitzTeamClass();

                if (newItem.id != null || newItem.id != "")
                {


                    if (newItem.country == null || newItem.country == "")
                    {
                        newItem.country = game.country;
                    }
                    if (newItem.teamname == null || newItem.teamname.Equals(""))
                    {
                        newItem.teamname = game.teamname;
                    }
              
                    if (newItem.teamimage == null || newItem.teamimage.Equals(""))
                    {
                        newItem.teamimage = game.teamimage;
                    }

                    if (newItem.owned_by == null || newItem.owned_by.Equals(""))
                    {
                        newItem.owned_by = game.owned_by;
                    }

                    if (newItem.teampoints.Equals(null) || newItem.teampoints == 0)
                    {
                        newItem.teampoints = game.teampoints;
                    }
                  await Api<tokblitzTeamClass>.UpdateItemAsyncSaveGames(game.id, newItem, Constants.PkRequest(game.id));
                    return new OkResult();
                }
                else
                {

                    return new BadRequestResult();

                }

            }
       catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);

            }
      }
















    }
}
