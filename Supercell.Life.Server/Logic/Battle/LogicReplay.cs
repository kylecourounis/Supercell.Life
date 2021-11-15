namespace Supercell.Life.Server.Logic.Battle
{
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands;

    internal class LogicReplay
    {
        internal LogicGameMode GameMode;

        internal LogicQuestData Quest;
        internal LogicEventsData Event;

        internal int LevelIndex;
        internal int Challenge;
        internal int StartingPlayer;

        internal LogicJSONObject Avatar;
        internal LogicJSONObject Avatar2;

        internal int EndTick;

        internal LogicArrayList<LogicCommand> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicReplay"/> class.
        /// </summary>
        internal LogicReplay(LogicGameMode gamemode)
        {
            this.GameMode = gamemode;
        }

        /// <summary>
        /// Returns a list of commands from the specified <see cref="LogicJSONArray"/>.
        /// </summary>
        internal static LogicArrayList<LogicCommand> GetCommands(LogicJSONArray array, Connection connection)
        {
            var commands = new LogicArrayList<LogicCommand>();

            for (var i = 0; i < array.Size; i++)
            {
                commands.Add(LogicCommandManager.LoadCommandFromJSON(array.GetJsonObject(i), connection));
            }

            return commands;
        }
    }
}
