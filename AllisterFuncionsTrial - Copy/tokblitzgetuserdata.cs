using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AllisterFuncionsTrial.Services;
using AllisterFuncionsTrial.Models;
using Microsoft.Azure.Documents.Client;

namespace AllisterFuncionsTrial
{
    public static class tokblitzgetuserdata
    {
        private const string resource = "knowledgeandusers";
        [FunctionName("GetInknowledgeandusers")]
        public static async Task<IActionResult> UserGetInknowledgeAndUsers([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + resource + "/{id}")]HttpRequest req, ILogger log,
           ExecutionContext context, string id)
        {
            try
            {
                var item = await GetUserAndKnowledge(id);

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
        
        public static async Task<dynamic> GetUserAndKnowledge(string id)
        {
            RequestOptions options;
            options = Constants.PkRequest(id);
            var  item = await Api<TokketUsers>.GetItemAsyncInUsers(id, options);

            ////Get {userlocalid}-counter
            ///
            var counter = await Api<UserCounter>.GetItemAsyncInKnowledge(id + "-counter", Constants.PkRequest(id + "-counter"));

            if (counter != null)
            {
                // mix and appen the data or user container and knowledge as one
                item.SetCounts(counter);
            }


            return item;
        }


         // when user gets the data, tokketuser class and Usercounter should have the same equivalent class to be fetched
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
                user.StrikesTokBlitz = (counter?.StrikesTokBlitz != null) ? (long)(counter?.StrikesTokBlitz) : 0;
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
                user.SavedTokBlast = (counter?.TokblastSaved != null) ? (long)(counter?.TokblastSaved) : 0;
                user.tokblitzNumTeam = (counter?.tokblitzNumTeam != null) ? (int)(counter?.tokblitzNumTeam) : 0;
                user.IsRoomPurchasedTokBlitz = (counter?.IsRoomPurchasedTokBlitz != null) ? (bool)(counter?.IsRoomPurchasedTokBlitz) : false;
            }

            return user;
        }






    }
}
                                                                                                     