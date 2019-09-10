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
using Microsoft.Azure.Documents.Linq;

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
            IEnumerable<tokblitzTeamClass> get_team = Teams.Where(x => x.UserId == getId);
            return new OkObjectResult(get_team);
        }

         // insert team created by the user
        [FunctionName("tokblitzinsertteam")]
        public static async Task<IActionResult> insertTeam([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/" + "insertteam") ]HttpRequest req, ILogger log,
         ExecutionContext context)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            tokblitzTeamClass getrequest = JsonConvert.DeserializeObject<tokblitzTeamClass>(requestBody);
            getrequest.UserId = getrequest.Id + "-tokblitzteam";
            getrequest.Id = getrequest.Id + Guid.NewGuid();

            var item = await Api<tokblitzTeamClass>.CreateItemAsyncGames(getrequest,Constants.PkRequest(getrequest.Id));
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

                if (newItem.Id != null || newItem.Id != "")
                {
                 tokblitzTeamClass itemtoupdate =  TeamClassUpdater<tokblitzTeamClass>.TeamUpdater(newItem,game);
                                                                

                  var result =  await Api<tokblitzTeamClass>.UpdateItemAsyncSaveGames(game.Id, itemtoupdate, Constants.PkRequest(game.Id));
                    return new OkObjectResult(result);
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


        // gets the teams of a particular user
        // just throw the user (localid) "select all wher owned_by = userlocalid-tokblitzteam";
        [FunctionName("tokblitzgetteamsbychoice")]
        public static async Task<IActionResult> tokblitzgetteamsbychoice([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + "getteamsbychoice/" + "{id}/"+"{topvalue}")]HttpRequest req, ILogger log,
        ExecutionContext context, string id)
        {
            // set the get id to (userlocalid)-tokblitzteam
            string getId = id + "-tokblitzteam";
            //IDocumentQuery<tokblitzTeamClass> query;
            //query = Constants.Client.CreateDocumentQuery<tokblitzTeamClass>(
            //    UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CollectionId),
            //    _crosspartition)
            //    .Where(x => x.Label == "team")
            //    .OrderByDescending(x => x.TeamPoints)
            //    .AsDocumentQuery();
            ////.Where(x => x.UserId == getId)
            ////.OrderByDescending(x => x.TeamPoints)
            ////.AsDocumentQuery();

            var sql = "SELECT TOP 100 * FROM Games Where Games.label ='team' Order By Games.team_points Desc";
            var query = Constants.Client
               .CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CollectionId), sql,
                _crosspartition)
               .AsDocumentQuery();

            List<tokblitzTeamClass> results = new List<tokblitzTeamClass>();
            List<tokblitzTeamClass> resultsForCaller = new List<tokblitzTeamClass>();
            List<int> getPoints = new List<int>();
            // put all the team in results

            
            if (query.HasMoreResults)
            {

                var result = await query.ExecuteNextAsync<tokblitzTeamClass>();
                var count = 1;
                foreach (var tt in result)
                {

                    tt.TeamRank = count;
                    count++;

                }
                results.AddRange(result);
                resultsForCaller.AddRange(results.Where(x => x.UserId == getId));

            }

          
             return new OkObjectResult(resultsForCaller);
            
        }















    }
}
