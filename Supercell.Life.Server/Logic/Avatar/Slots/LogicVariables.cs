namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicVariables : LogicDataSlot
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
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
        }
    }
}
