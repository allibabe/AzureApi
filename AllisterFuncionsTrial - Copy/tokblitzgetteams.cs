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

namespace AllisterFuncionsTrial
{
    public static class tokblitzgetteams
    {
        private static FeedOptions getalli = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

        [FunctionName("tokblitzgetteams")]
        public static async Task<IActionResult> getter([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/" + "try" )]HttpRequest req, ILogger log,
         ExecutionContext context)
        {

            //var alligetter = await Api<dynamic>.getall();


               IQueryable<UserCounter> family = Constants.Client.CreateDocumentQuery<UserCounter>(
               UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId,Constants.CollectionIdProdUsers),getalli);

            //var a = ((IEnumerable)family).Cast<dynamic>()
            //                .Where(p => p.id == "1tULByrYkkdXQzvRVR3EZqavKZh1");

            //List<dynamic> gets = JsonConvert.DeserializeObject<List<dynamic>>(family);

            IEnumerable<UserCounter> getterlast = family.Where(x => x.Likes == 1);
                               



            //var tt = from name in family
            //         where name.('a')
            //         select name;

            return new OkObjectResult(getterlast);
        }

       
        public static UserCounter GetItemAsync()
        {
            IQueryable<UserCounter> familyQuery = Constants.Client.CreateDocumentQuery<UserCounter>(
            UriFactory.CreateDocumentCollectionUri(Constants.DatabaseIdProd, Constants.CollectionIdProd), null)
           .Where(f => f.Id == "1tULByrYkkdXQzvRVR3EZqavKZh1-counter");
            var all =  familyQuery;
                            
            return familyQuery.FirstOrDefault(x => x.Id== "1tULByrYkkdXQzvRVR3EZqavKZh1-counter");
        }

    }
}
