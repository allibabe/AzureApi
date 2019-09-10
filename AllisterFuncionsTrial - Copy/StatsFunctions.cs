using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TokGamesApi;
using AllisterFuncionsTrial.Models;
using AllisterFuncionsTrial.Services;
using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace AllisterFuncionsTrial
{
    public static class StatsFunctions
    {

        private const string resource = "";

        #region DayCounter
        [FunctionName("DayCounterGet")]
        public static async Task<IActionResult> DayCounterGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/daycounter")]HttpRequest req, ILogger log,
            ExecutionContext context)
        {
            try
            {
                var item = await GetDayCounter();
                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        public static async Task<DayCounter> GetDayCounter()
        {
            var dateId = $"{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}";
            var counterId = $"daycounter-{dateId}";

            var item = await Api<DayCounter>.GetItemAsync(counterId, Constants.PkRequest(counterId));

            if (item == null)
                item = new DayCounter() { Id = counterId, Messages = 0 };

            return item;
        }

        public static async Task<DayCounter> IncrementDayCounter(long count)
        {
            var dateId = $"{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}";
            var counterId = $"daycounter-{dateId}";
            DayCounter counter = null;

            bool success = false;
            while (!success)
            {
                try
                {
                    counter = await Api<DayCounter>.IncrementCountAsync(counterId, StringExtensions.BuildUpdateString("$inc", new Dictionary<string, long>() { { "messages", count } }), Constants.PkRequest(counterId));
                    return counter;
                }
                catch (DocumentClientException e)
                {
                    var error = e.Message;

                    if (error.Contains("Document not found."))
                    {
                        counter = new DayCounter() { Id = counterId, Messages = 1 };
                        await Api<DayCounter>.CreateItemAsync(counter, Constants.PkRequest(counterId));
                        return counter;
                    }
                    success = false;
                }
                catch (Exception ex)
                {
                    success = false;
                }
            }
            return counter;
        }

        #endregion
    }


}

