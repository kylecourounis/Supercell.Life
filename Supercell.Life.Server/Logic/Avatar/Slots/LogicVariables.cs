namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicVariables : LogicDataSlots
    {
        #region VariableData

        internal static LogicVariableData Wins                 = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("PVPWinCount");
        internal static LogicVariableData WinStreak            = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("WinStreak");
        internal static LogicVariableData PerfectPVP           = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("PerfectPVPCount");
        internal static LogicVariableData Matches              = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("PVPMatchCount");
        internal static LogicVariableData ChestProgress        = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("PvpChestProgress");
        internal static LogicVariableData ChestProgressUpdated = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("PvpChestProgressUPDATED");
        internal static LogicVariableData SpellSlotsUnlocked   = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("SpellSlotsUnlocked");
        internal static LogicVariableData SailRewardUnclaimed  = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("SailRewardUnclaimed");
        internal static LogicVariableData SailCount            = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("SailCount");
        internal static LogicVariableData IgnoreOngoingQuest   = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("IgnoreOngoingQuest");
        internal static LogicVariableData IntroSeen            = (LogicVariableData)CSV.Tables.Get(Gamefile.Variables).GetDataByName("IntroSeen");

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicVariables"/> class.
        /// </summary>
        internal LogicVariables(LogicClientAvatar avatar) : base(avatar, 25)
        {
            // LogicVariables.
        }

        /// <summary>
        /// Updates the variables relating to a multiplayer match.
        /// </summary>
        internal void UpdatePvPVariables(bool didWin)
        {
            this.AddItem(LogicVariables.Matches.GlobalID, 1);

            if (didWin)
            {
                this.AddItem(LogicVariables.Wins.GlobalID, 1);
                this.AddItem(LogicVariables.WinStreak.GlobalID, 1);

                this.AddChestProgress();
            }
            else
            {
                this.Set(LogicVariables.WinStreak.GlobalID, 0);
            }
        }

        /// <summary>
        /// Adds the specified count to the PvP chest progress variables.
        /// </summary>
        internal void AddChestProgress(int count = 1)
        {
            this.AddItem(LogicVariables.ChestProgress.GlobalID, count);
            this.AddItem(LogicVariables.ChestProgressUpdated.GlobalID, count);
        }

        /// <summary>
        /// Sets PvP chest progress variables to zero.
        /// </summary>
        internal void ResetChestProgress()
        {
            this.Set(LogicVariables.ChestProgress.GlobalID, 0);
            this.Set(LogicVariables.ChestProgressUpdated.GlobalID, 0);
        }

        /// <summary>
        /// Returns a value that indicates whether the player has completed the PvP chest.
        /// </summary>
        internal bool IsPvPChestProgressComplete()
        {
            return this.Get(LogicVariables.ChestProgress.GlobalID).Count == 5 && this.Get(LogicVariables.ChestProgressUpdated.GlobalID).Count == 5;
        }
    }
}
