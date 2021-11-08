namespace Supercell.Life.Server.Helpers
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects;

    internal static class AvatarExtensions
    {
        /// <summary>
        /// Called when the player wins a battle.
        /// </summary>
        internal static void WinBattle(this LogicClientAvatar avatar, int trophies = 30)
        {
            LogicLeagueData league = (LogicLeagueData)CSV.Tables.Get(Gamefile.Leagues).GetDataWithID(avatar.League);

            avatar.CommodityChangeCountHelper(LogicCommodityType.Gold, league.PVPGoldReward);
            avatar.CommodityChangeCountHelper(LogicCommodityType.Experience, league.PVPXpReward);

            avatar.AddTrophies(trophies);

            avatar.Variables.AddItem(LogicVariables.Wins.GlobalID, 1);
            avatar.Variables.AddItem(LogicVariables.WinStreak.GlobalID, 1);
            avatar.Variables.AddItem(LogicVariables.Matches.GlobalID, 1);

            avatar.Variables.AddItem(LogicVariables.ChestProgress.GlobalID, 1);
            avatar.Variables.AddItem(LogicVariables.ChestProgressUpdated.GlobalID, 1);

            if (avatar.Variables.Get(LogicVariables.ChestProgress.GlobalID).Count == 5 && avatar.Variables.Get(LogicVariables.ChestProgressUpdated.GlobalID).Count == 5)
            {
                avatar.Variables.Set(LogicVariables.ChestProgress.GlobalID, 0);
                avatar.Variables.Set(LogicVariables.ChestProgressUpdated.GlobalID, 0);

                LogicChest chest = new LogicChest(avatar);
                chest.CreateMegaChest();
            }
        }

        /// <summary>
        /// Called when the player loses a battle.
        /// </summary>
        internal static void LoseBattle(this LogicClientAvatar avatar)
        {
            avatar.LoseTrophies(30);
            avatar.Resigned = false;
        }

        /// <summary>
        /// Adds the trophies and updates the player's league accordingly.
        /// </summary>
        internal static void AddTrophies(this LogicClientAvatar avatar, int value = 30)
        {
            if (value > 0)
            {
                avatar.Score += value;

                if (avatar.Score < 2500)
                {
                    LogicLeagueData league = (LogicLeagueData)CSV.Tables.Get(Gamefile.Leagues).GetDataWithID(avatar.League);

                    if (avatar.Score > league.PromoteLimit - 1)
                    {
                        avatar.League += 1;
                    }
                }

                avatar.Save();
            }
        }

        /// <summary>
        /// Removes the trophies and updates the player's league accordingly.
        /// </summary>
        internal static void LoseTrophies(this LogicClientAvatar avatar, int value = 10)
        {
            if (value > 0 && avatar.Score > 0)
            {
                if (value > avatar.Score)
                {
                    avatar.Score = 0;
                }
                else
                {
                    avatar.Score -= value;
                }

                LogicLeagueData league = (LogicLeagueData)CSV.Tables.Get(Gamefile.Leagues).GetDataWithID(avatar.League);

                if (avatar.Score < league.PlacementLimitLow)
                {
                    avatar.League -= 1;
                }

                avatar.Save();
            }
        }
        
        /// <summary>
        /// Sets the rank of the specified player.
        /// </summary>
        internal static bool SetRank(this LogicClientAvatar avatar, Rank rank)
        {
            if (rank > avatar.Rank)
            {
                avatar.Rank = rank;
                Debugger.Debug($"{avatar.Name} is now a {avatar.Rank}!");

                avatar.Save();

                return true;
            }

            Debugger.Error($"{avatar.Rank} was the less than or equal to {avatar.Name}'s current rank!");

            return false;
        }
    }
}
