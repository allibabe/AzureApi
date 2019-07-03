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

namespace AllisterFuncionsTrial
{
    public static class coinsandstrikesupdater
    {
         
        
        /// <summary>
        ///  this function is for updating the coins or strikes
        ///  the item being thrown on this api are (id) in the query string and 
        ///  eg{
        //              "id": userLocalID,   --- userlocalid of the user to be concatinated with "-counter"
        //              "coins": 0,   ----- the current value to put
        //              "tokblitz_strikes": --- the current strikes to put
        ///// 
        ///     }
        /// </summary>
        private const string resource = "CoinsStrikes";
        [FunctionName("CoinsAndStrikes")]
        public static async Task<IActionResult> CoinsStrikesUpdate([HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Version + "/" + resource + "/{id}")]HttpRequest req, ILogger log,
          ExecutionContext context, string id)
        {
            try
            {
                // check and get the data using the localid in the requestbody appending "-counter"
                var item = await Api<dynamic>.GetItemAsyncInKnowledge(id + "-counter", Constants.PkRequest(id + "-counter"));
                
                // will hold the data of the response if there is any
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

                // updating the value of strikes
                item.tokblitz_strikes = newItem.tokblitz_strikes;
                await Api<dynamic>.UpdateItemAsync(item.id, item, Constants.PkRequest(item.pk));
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
       
    }
}
