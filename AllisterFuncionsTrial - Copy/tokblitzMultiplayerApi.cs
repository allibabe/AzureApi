using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using AllisterFuncionsTrial.Services;
using AllisterFuncionsTrial.Models;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using TokGamesApi;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using System.Security.Claims;

namespace AllisterFuncionsTrial
{
    public static class tokblitzMultiplayerApi
    {
        //[FunctionName("tokblitzMultiplayerApi")]
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

        [FunctionName("MultiplayerConnect")]
        public static async Task<SignalRConnectionInfo> GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/multiplayerconnect")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "multiplayer")] SignalRConnectionInfo connectionInfo)
        {
            var counter = await StatsFunctions.GetDayCounter();

            if (counter.Messages > Constants.DAILY_MSGS_MAX)
                return null;
            else
                return connectionInfo;
        }

       

        //    [FunctionName("negotiate")]
        //    public static SignalRConnectionInfo Negotiate(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version + "/negotiate")]HttpRequest req,
        //[SignalRConnectionInfo
        //    (HubName = "multiplayer") ]
        //    SignalRConnectionInfo connectionInfo)
        //    {

        //        // connectionInfo contains an access key token with a name identifier claim set to the authenticated user
        //        return connectionInfo;
        //    }


        //    [FunctionName("addToGroup")]
        //    public static async Task<IActionResult> AddToGroup(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version + "/addgroup/" + "{id}")]HttpRequest req,
        //string id,
        //[SignalR(HubName = "chat")]
        //    IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        //    {


        //        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //        var item = JsonConvert.DeserializeObject<Group>(requestBody);
        //        await signalRGroupActions.AddAsync(
        //            new SignalRGroupAction
        //            {
        //                UserId = id,
        //                GroupName = item.Group_name,
        //                Action = GroupAction.Add
        //            });

        //        return new OkObjectResult(id);
        //    }


        //    [FunctionName("removeFromGroup")]
        //    public static async Task<IActionResult> RemoveFromGroup(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version + "/removeuser" + "/{userId}")]HttpRequest req,
        //string userId,
        //[SignalR(HubName = "chat")]
        //    IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        //    {

        //        //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //        //var item = JsonConvert.DeserializeObject<Group>(requestBody);

        //        await signalRGroupActions.AddAsync(
        //         new SignalRGroupAction
        //         {
        //             UserId = userId,
        //             GroupName = "group1",
        //             Action = GroupAction.Remove
        //         });

        //        return new OkResult();
        //    }


        //    [FunctionName("SendGroupMessage")]
        //    public static async Task<IActionResult> SendGroupMessage(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.Version + "/SendGroupMessage" + "/{message}")]HttpRequest req, string message,
        //    [SignalR(HubName = "chat")]IAsyncCollector<SignalRMessage> signalRMessages)
        //    {
        //        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //        var item = JsonConvert.DeserializeObject<Group>(requestBody);
        //        await signalRMessages.AddAsync(
        //           new SignalRMessage
        //           {
        //               // the message will be sent to the group with this name
        //               // GroupName = item.Group_name,
        //               //   UserId = message,
        //               Target = "newMessage",
        //               Arguments = new[] { message }
        //           });

        //        return new OkObjectResult(message);
        //    }



        [FunctionName("LobbyGet")]
        public static async Task<IActionResult> LobbyGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Version + "/lobby")]HttpRequest req, ILogger log,
            ExecutionContext context)
        {
            try
            {
                var item = await GetLobby();
                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        public static async Task<TokGamesRoomBasic> GetLobby()
        {
            var lobby = await Api<TokGamesRoomBasic>.GetItemAsync("lobby", Constants.PkRequest("lobby"));
            return lobby;
        }

        [FunctionName("MultiplayerSetup")]
        public static async Task<IActionResult> MultiplayerSetup(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/multiplayersetup")] HttpRequest req,
            [SignalR(HubName = "multiplayer")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            //Read query info
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var player = JsonConvert.DeserializeObject<GamePlayer>(requestBody);

            //Get list of players
            Constants.CollectionId = "Games";
            var lobby = await Api<TokGamesRoomBasic>.GetItemAsync("lobby", Constants.PkRequest("lobby"));
            lobby.WaitingPlayers.RemoveAll(x => x.UserId == player.UserId);

            if (lobby.WaitingPlayers.Count == 0)
            {   //Wait for other player
                lobby.WaitingPlayers.Add(player);


                await Api<TokGamesRoomBasic>.UpdateItemAsyncSaveGames("lobby", lobby, Constants.PkRequest("lobby"));

                await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "waiting",
                    Arguments = new[] { player }
                });
                // await StatsFunctions.IncrementDayCounter(1);
            }
            else
            {
                //Match up with a waiting player
                var player1 = player;
                var player2 = lobby.WaitingPlayers.FirstOrDefault();

                TokBlitzRace gameInfo = new TokBlitzRace();
                player1.PlayerNumber = 1;
                player2.PlayerNumber = 2;
                gameInfo.Players.Add(player1);
                gameInfo.Players.Add(player2);
                gameInfo.RoomId = Guid.NewGuid().ToString();

                //Get game toks
                gameInfo.Toks = GetGameTokIds();

                //Remove once successful
                lobby.WaitingPlayers.RemoveAll(x => x.UserId == player2.UserId);
                await Api<TokGamesRoomBasic>.UpdateItemAsyncSaveGames("lobby", lobby, Constants.PkRequest("lobby"));

                await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newgame",

                    Arguments = new[] { gameInfo }

                });
                // await StatsFunctions.IncrementDayCounter(1);
            }

            return new OkResult();
        }



        public static List<int> GetGameTokIds()
        {
            int[] ids = new int[3600];
            for (int i = 0; i < ids.Length; ++i)
            {
                ids[i] = i + 1;
            }
            return RandomExtensions<int>.Randomize(ids).Take(7).ToList();
        }
                    

        [FunctionName("MultiplayerStopWaiting")]
        public static async Task<IActionResult> MultiplayerStopWaiting(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/multiplayerstopwaiting")] HttpRequest req,
            [SignalR(HubName = "multiplayer")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            //Read query info
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var player = JsonConvert.DeserializeObject<GamePlayer>(requestBody);

            //Get list of players
            var lobby = await Api<TokGamesRoomBasic>.GetItemAsync("lobby", Constants.PkRequest("lobby"));

            //Remove if still in waiting list
            lobby.WaitingPlayers.RemoveAll(x => x.ConnectionId == player.ConnectionId);

            //Update lobby
            await Api<TokGamesRoomBasic>.UpdateItemAsyncSaveGames("lobby", lobby, Constants.PkRequest("lobby"));
            return new OkObjectResult(lobby); 
        
          }



        [FunctionName("MultiplayerSendGameMessage")]
        public static async Task SendMessage(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/sendgamemessage")] HttpRequest req,
            [SignalR(HubName = "multiplayer")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            //Read query info
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var message = JsonConvert.DeserializeObject<GameMessage>(requestBody);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = message.Type,
                    Arguments = new[] { message }
                });
            await StatsFunctions.IncrementDayCounter(1);
        }

        private static FeedOptions _crosspartition = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
        [FunctionName("TokBlitzRaceGameOver")]
        public static async Task<IActionResult> TokBlitzRaceGameOver(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/tokblitzracegameover")] HttpRequest req,
            [SignalR(HubName = "multiplayer")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            //Read query info
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var info = JsonConvert.DeserializeObject<TokBlitzRace>(requestBody);

            bool forfeit = false;
            if (info.ForfeitingPlayer != null)
            {
                info.Loser = info.ForfeitingPlayer;
                info.LoserId = info.ForfeiterId;
                info.Winner = (info.ForfeitingPlayer.PlayerNumber == 1 ) ? info.Players[0]: info.Players[1];
                info.WinnerId = info.Winner.ConnectionId;
              
                forfeit = true;

            }
            else
            {

                if (info.FinishedFirst.PlayerNumber == 1)
                {
                    info.Player1Points += Constants.FINISHED_FIRST_BONUS;
                    if (info.Player1PointsPerRound != null)
                        info.Player1PointsPerRound.Add(Constants.FINISHED_FIRST_BONUS);
                }
                else if (info.FinishedFirst.PlayerNumber == 2)
                {
                    info.Player2Points += Constants.FINISHED_FIRST_BONUS;
                    if (info.Player2PointsPerRound != null)
                        info.Player2PointsPerRound.Add(Constants.FINISHED_FIRST_BONUS);
                }

                info.Winner = (info.Player1Points > info.Player2Points) ? info.Players[0] : info.Players[1];
                info.WinnerId = info.Winner.ConnectionId;

                info.Loser = (info.Player1Points > info.Player2Points) ? info.Players[1] : info.Players[0];
                info.LoserId = info.Loser.ConnectionId;
                if (info.Player1Points == info.Player2Points) info.Winner = new GamePlayer() { PlayerNumber = 0, UserId = "Tie", UserName = "Tie", UserPhoto = "Tie" };

      
           }

            // update the points and others

            tokblitzTeamClass getwinner = new tokblitzTeamClass();
            getwinner.Id = info.Winner.ConnectionId;
            getwinner.GamesPlayed = 1;
            getwinner.Wins = 1;
            getwinner.TeamPoints = 2;

           
            tokblitzTeamClass getloser = new tokblitzTeamClass();
            getloser.Id = info.Loser.ConnectionId;
            getloser.GamesPlayed = 1;
            getloser.Losses = 1;
            getloser.TeamPoints = (forfeit == true) ? 0 : 1;

            tokblitzTeamClass getwinnerOld =  await Api<tokblitzTeamClass>.GetItemAsync(getwinner.Id,Constants.PkRequest(getwinner.Id));
            tokblitzTeamClass getwinnerTosend = TeamClassUpdater<tokblitzTeamClass>.TeamUpdater(getwinner,getwinnerOld);

            tokblitzTeamClass getLoserOld = await Api<tokblitzTeamClass>.GetItemAsync(getloser.Id, Constants.PkRequest(getloser.Id));
            tokblitzTeamClass getLoserTosend = TeamClassUpdater<tokblitzTeamClass>.TeamUpdater(getloser, getLoserOld);


            await Api<tokblitzTeamClass>.UpdateItemAsyncSaveGames(getwinner.Id,getwinnerTosend, Constants.PkRequest(getwinner.Id));
            await Api<tokblitzTeamClass>.UpdateItemAsyncSaveGames(getloser.Id, getLoserTosend, Constants.PkRequest(getloser.Id));


            // creates a rematch record after the game
            RematchRecord rematchRecord = new RematchRecord();
            Guid newId;
            newId = Guid.NewGuid();
            rematchRecord.Id = newId.ToString();
            rematchRecord.Pk = newId.ToString();
            rematchRecord.Player1ConnectionId = info.Players[0].ConnectionId.ToString();
            rematchRecord.Player2ConnectionId = info.Players[1].ConnectionId.ToString();
            rematchRecord.Player1Id = info.Players[0].UserId.ToString();
            rematchRecord.Player2Id = info.Players[1].UserId.ToString();
          
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "tokblitzracegameover",
                    Arguments = new[] { info }
                });
                    

            await StatsFunctions.IncrementDayCounter(1);

            
            await Api<TokBlitzRace>.CreateItemAsync(info, Constants.PkRequest(info.Id));
            await Api<RematchRecord>.CreateItemAsync(rematchRecord,Constants.PkRequest(rematchRecord.Id));

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "rematch_record",
                    Arguments = new[] { rematchRecord}
                });



            return new OkObjectResult(info);
        }


        
        [FunctionName("tokblitzracerematch")]
        public static async Task<IActionResult> TokblitzRaceRematch(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Version + "/tokblitzracerematch/" +"{id}/" + "{answer}")] HttpRequest req, string id,bool answer,
          [SignalR(HubName = "multiplayer")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            //Read query info
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // creates a new gameplyer from rquest body
            var Record = JsonConvert.DeserializeObject<GamePlayer>(requestBody);

            //Get the record rematch and put in the FetchedRecord variable
            Constants.CollectionId = "Games";
            var FetchedRecord = await Api<RematchRecord>.GetItemAsync(id, Constants.PkRequest(id));

            // checks if the userid of the one asking for rematch is the same with the userId player1 in the rematch record
            if (Record.UserId == FetchedRecord.Player1Id)
            {
                if (answer == true) {
                    // if true,  update the rematch record set the value of player1rematch to "true"
                    FetchedRecord.Player1Rematch = true;
                    // if yes,  update the rematch record set the value of player1rematch to "true"
                    FetchedRecord.Player1HasMadeDecision = true;
                    await Api<RematchRecord>.UpdateItemAsyncSaveGames(FetchedRecord.Id, FetchedRecord, Constants.PkRequest(FetchedRecord.Id));

                }
                else {

                    // if false,  update the rematch record set the value of player1rematch to "false"
                    FetchedRecord.Player1Rematch = false;
                    // if false,  update the rematch record set the value of player1rematch to "true"
                    FetchedRecord.Player1HasMadeDecision = true;
                    await Api<RematchRecord>.UpdateItemAsyncSaveGames(FetchedRecord.Id, FetchedRecord, Constants.PkRequest(FetchedRecord.Id));

                }


            }
            else if (Record.UserId == FetchedRecord.Player2Id)
            {

                if (answer == true)
                {
                    // if true,  update the rematch record set the value of player1rematch to "true"
                    FetchedRecord.Player2Rematch = true;
                    // if yes,  update the rematch record set the value of player1rematch to "true"
                    FetchedRecord.Player2HasMadeDecision = true;
                    await Api<RematchRecord>.UpdateItemAsyncSaveGames(FetchedRecord.Id, FetchedRecord, Constants.PkRequest(FetchedRecord.Id));

                }
                else
                {

                    // if false,  update the rematch record set the value of player1rematch to "false"
                    FetchedRecord.Player2Rematch = false;
                    // if false,  update the rematch record set the value of player1rematch to "true"
                    FetchedRecord.Player2HasMadeDecision = true;
                    await Api<RematchRecord>.UpdateItemAsyncSaveGames(FetchedRecord.Id, FetchedRecord, Constants.PkRequest(FetchedRecord.Id));

                }


            }


            var UpdatedFetchedRecord = await Api<RematchRecord>.GetItemAsync(id, Constants.PkRequest(id));


            // if both have decided
            if (UpdatedFetchedRecord.Player1HasMadeDecision == true && UpdatedFetchedRecord.Player2HasMadeDecision == true)
            {
                // if both yes
                if (UpdatedFetchedRecord.Player1Rematch == true && UpdatedFetchedRecord.Player2Rematch == true)
                {


                    TokBlitzRace gameInfo = new TokBlitzRace();
                    var play1 = UpdatedFetchedRecord.Players[0];
                    play1.PlayerNumber = 1;
                    Record.PlayerNumber = 2;
                    //var play2 = UpdatedFetchedRecord.Players[1];
                    gameInfo.Players.Add(play1);
                    gameInfo.Players.Add(Record);
                    gameInfo.RoomId = Guid.NewGuid().ToString();

                    UpdatedFetchedRecord.Player1Rematch = false;
                    UpdatedFetchedRecord.Player2Rematch = false;
                    UpdatedFetchedRecord.Player2HasMadeDecision = false;
                    UpdatedFetchedRecord.Player1HasMadeDecision = false;


                    await Api<RematchRecord>.UpdateItemAsyncSaveGames(UpdatedFetchedRecord.Id, UpdatedFetchedRecord, Constants.PkRequest(UpdatedFetchedRecord.Id));


                    //Get game toks
                    gameInfo.Toks = GetGameTokIds();
                    await signalRMessages.AddAsync(
                           new SignalRMessage
                           {
                               Target = "new_game_rematch",
                               Arguments = new[] { gameInfo }
                           });
                           

                    // if both have decided and player 1 is false and player 2 is true
                }
                    else if (UpdatedFetchedRecord.Player1Rematch == false && UpdatedFetchedRecord.Player2Rematch == true)
                {
                    var getter = UpdatedFetchedRecord.Player2ConnectionId;
                    await signalRMessages.AddAsync(
                     new SignalRMessage
                     {
                         Target = "new_game_rematch_denied",
                         Arguments = new[] { getter }
                     });


                    // if both have decided and player 2 is false and player 1 is true
                }
                else if (UpdatedFetchedRecord.Player2Rematch == false && UpdatedFetchedRecord.Player1Rematch == true)
                 {

                        var getter = UpdatedFetchedRecord.Player1ConnectionId;
                        await signalRMessages.AddAsync(
                         new SignalRMessage
                         {
                             Target = "new_game_rematch_denied",
                             Arguments = new[] { getter }
                         });
                     

                  }

                   
            }
            // if only one player has decided 
            else
            {

                UpdatedFetchedRecord.Players.Add(Record);

                var updated = await Api<RematchRecord>.UpdateItemAsyncSaveGames(UpdatedFetchedRecord.Id, UpdatedFetchedRecord, Constants.PkRequest(UpdatedFetchedRecord.Id));

                await signalRMessages.AddAsync(
                       new SignalRMessage
                       {
                           Target = "waiting_for_rematch",
                           Arguments = new[] { "Please Wait for your opponent" }
                       });

                return new OkObjectResult(updated);

            }

                
            return new OkResult();
     
          
        }




    }     
}


  

public class Group
    {
        [JsonProperty("group_name")]
        public string Group_name { get; set; }

    }


public class AskingForRematch
{
    [JsonProperty("id")]
    public string id { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }
                        
}


//Message types:
//p1_round, p2_round, p1_guess, p2_guess, p1_points, p2_points, time
//Room Id is determined at the 



public class GameMessage
{

    [JsonProperty("player")]
    public GamePlayer Player { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("room_id")]
    public string RoomId { get; set; }

    [JsonProperty("content")]
    public dynamic Content { get; set; }

}



public class GameInfo
{
    [JsonProperty("id")]
    public string Id { get; set; } = "gameinfo-" + Guid.NewGuid().ToString();

    [JsonProperty("players")]
    public List<GamePlayer> Players { get; set; } = new List<GamePlayer>();

    [JsonProperty("room_id")]
    public string RoomId { get; set; }

    [JsonProperty("toks")]
    public List<int> Toks { get; set; }

    //[JsonProperty("toks")]
    //public List<GameTok> Toks { get; set; }

    [JsonProperty("content")]
    public dynamic Content { get; set; }
}



public class TokBlitzRace : GameInfo
{
    [JsonProperty("player1_points")]
    public int Player1Points { get; set; }

    [JsonProperty("player2_points")]
    public int Player2Points { get; set; }

    [JsonProperty("player1_points_per_round")]
    public List<int> Player1PointsPerRound { get; set; }

    [JsonProperty("player2_points_per_round")]
    public List<int> Player2PointsPerRound { get; set; }

    [JsonProperty("player1_guess")]
    public int Player1Guess { get; set; }

    [JsonProperty("player2_guess")]
    public int Player2Guess { get; set; }

    // new 
    [JsonProperty("player1_strikes_per_round")]
    public List<int> Player1_strikes_per_round { get; set; }

    [JsonProperty("player2_strikes_per_round")]
    public List<int> Player2_strikes_per_round { get; set; }
     // new
    [JsonProperty("player1_seconds_per_round")]
    public List<int> Player1_seconds_per_round { get; set; }

    [JsonProperty("player2_seconds_per_round")]
    public List<int> Player2_seconds_per_round { get; set; }
    // new
    [JsonProperty("player1_guesses_per_round")]
    public List<int> Player1_guesses_per_round { get; set; }
    
    [JsonProperty("player2_guesses_per_round")]
    public List<int> Player2_guesses_per_round { get; set; }

    [JsonProperty("player1_round")]
    public int Player1Round { get; set; }

    [JsonProperty("player2_round")]
    public int Player2Round { get; set; }

    [JsonProperty("seconds_elapsed")]
    public int SecondsElasped { get; set; }

    [JsonProperty("finished_first")]
    public GamePlayer FinishedFirst { get; set; }
   

    [JsonProperty("strikes")]
    public int Strikes { get; set; } = 12;

    [JsonIgnore]
    [JsonProperty("forfeiting_player")]
    public GamePlayer ForfeitingPlayer { get; set; } = null;

    [JsonProperty(PropertyName = "forfeiter_id", NullValueHandling = NullValueHandling.Ignore)]
    public string ForfeiterId { get; set; } = null;

    [JsonProperty("winner_id")]
    public string WinnerId { get; set; }

    [JsonProperty("loser_id")]
    public string LoserId { get; set; } = null;

    [JsonIgnore]
    public GamePlayer Winner { get; set; }

    [JsonIgnore]
    public GamePlayer Loser { get; set; } = null;



}




public class GamePlayer
{
    [JsonProperty("user_connection_id")]
    public string ConnectionId { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("user_name")]
    public string UserName { get; set; }

    [JsonProperty("user_photo")]
    public string UserPhoto { get; set; }

    [JsonProperty("user_flag")]
    public string UserFlag { get; set; }

    [JsonProperty("player_number", NullValueHandling = NullValueHandling.Ignore)]
    public int? PlayerNumber { get; set; } = null;


}
