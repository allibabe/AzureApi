using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AllisterFuncionsTrial.Models;

namespace AllisterFuncionsTrial.Services
{
   public static class TeamClassUpdater<T> where T :class 
    {
             // updates the value of team fields
        public static tokblitzTeamClass TeamUpdater(tokblitzTeamClass newItem, tokblitzTeamClass oldItem)
        {
           
                if (newItem.Country == null || newItem.Country == "")
                {
                    newItem.Country = oldItem.Country;
                }

                if (newItem.State == null || newItem.State == "")
                  {
                        newItem.State = oldItem.State;
                 }
            

            if (newItem.Name == null || newItem.Name.Equals(""))
                {
                    newItem.Name = oldItem.Name;
                }

                if (newItem.TeamImage == null || newItem.TeamImage.Equals(""))
                {
                    newItem.TeamImage = oldItem.TeamImage;
                }

                if (newItem.UserId == null || newItem.UserId.Equals(""))
                {
                    newItem.UserId = oldItem.UserId;
                }
                // updates points if not null or 0
                if (newItem.TeamPoints.Equals(null) || newItem.TeamPoints == 0)
                {
                    newItem.TeamPoints = oldItem.TeamPoints;
                }
                else
                {
                    newItem.TeamPoints = (oldItem.TeamPoints + newItem.TeamPoints);
                }
                // calculates wins
                if (newItem.Wins.Equals(null) || newItem.Wins == 0)
                {
                    newItem.Wins = oldItem.Wins;
                }
                else
                {
                    newItem.Wins = (oldItem.Wins + newItem.Wins);
                }

                // calculates losses
                if (newItem.Losses.Equals(null) || newItem.Losses == 0)
                {
                    newItem.Losses = oldItem.Losses;
                }
                else
                {
                    newItem.Losses = (oldItem.Losses + newItem.Losses);
                }

                // calculates strikes used
                if (newItem.StrikesUsed.Equals(null) || newItem.StrikesUsed == 0)
                {
                    newItem.StrikesUsed = oldItem.StrikesUsed;
                }
                else
                {
                    newItem.StrikesUsed = (oldItem.StrikesUsed + newItem.StrikesUsed);
                }
                //calculates  and update total seconds
                if (newItem.TotalSeconds.Equals(null) || newItem.TotalSeconds == 0)
                {
                    newItem.TotalSeconds = oldItem.TotalSeconds;
                }
                else
                {
                    newItem.TotalSeconds = (oldItem.TotalSeconds + newItem.TotalSeconds);
                }

                //calculates  and update total  games played
                if (newItem.GamesPlayed.Equals(null) || newItem.GamesPlayed == 0)
                {
                    newItem.GamesPlayed = oldItem.GamesPlayed;
                }
                else
                {
                    newItem.GamesPlayed = (oldItem.GamesPlayed + newItem.GamesPlayed);
                }
              
                return newItem;

                }


        }
    }
