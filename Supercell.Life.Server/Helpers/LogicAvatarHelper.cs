namespace Supercell.Life.Server.Helpers
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects;

    internal static class LogicAvatarHelper
    {
        /// <summary>
        /// Called when the player wins a battle.
        /// </summary>
        internal static void WinBattle(this LogicClientAvatar avatar)
        {
            LogicLeagueData league = (LogicLeagueData)CSV.Tables.Get(Gamefile.Leagues).GetDataWithID(avatar.League);

            avatar.CommodityChangeCountHelper(CommodityType.Gold, league.PVPGoldReward);
            avatar.CommodityChangeCountHelper(CommodityType.Experience, league.PVPXpReward);
            
            avatar.Variables.UpdatePvPVariables(true);

            if (avatar.Variables.IsPvPChestProgressComplete())
            {
                avatar.Variables.ResetChestProgress();

                LogicChest chest = new LogicChest(avatar);
                chest.CreateMegaChest();
            }
        }

        /// <summary>
        /// Called when the player loses a battle.
        /// </summary>
        internal static void LoseBattle(this LogicClientAvatar avatar)
        {
            avatar.Variables.UpdatePvPVariables(false);

            avatar.Connection.GameMode.Resigned = false;
        }

        /// <summary>
        /// Adds the trophies and updates the player's league accordingly.
        /// </summary>
        internal static void AddTrophyScoreHelper(this LogicClientAvatar avatar, int value)
        {
            avatar.Score += value;

            LogicLeagueData league = (LogicLeagueData)CSV.Tables.Get(Gamefile.Leagues).GetDataWithID(avatar.League);

            if (value > 0)
            {
                if (avatar.League < 11)
                {
                    if (avatar.Score >= league.PromoteLimit)
                    {
                        avatar.League += 1;
                    }
                }
            }
            else
            {
                if (avatar.Score < 0)
                {
                    avatar.Score = 0;
                }

                if (avatar.Score <= league.DemoteLimit)
                {
                    avatar.League -= 1;
                }
            }

            avatar.Save();
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
