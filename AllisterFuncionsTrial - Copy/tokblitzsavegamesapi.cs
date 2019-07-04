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

namespace AllisterFuncionsTrial
{
    public static class tokblitzsavegamesapi
    {
        [FunctionName("tokblitzsavegamesapi")]
        public static async Task<IActionResult> tokblitzsavegameapi([HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "tokblitzsavegame")]HttpRequest req, ILogger log,
          ExecutionContext context)
        {

            //await Api<newSavedGames>.DeleteItemAsync(id, Constants.PkRequest(id));

            return new OkResult();
        }



    }
}


    public class save_games_class{

    public string id { get; set; }

}
