namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;
    using Supercell.Life.Server.Network;

    internal class LogicStartQuestCommand : LogicCommand
    {
        internal LogicQuestData QuestData;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartQuestCommand"/> class.
        /// </summary>
        public LogicStartQuestCommand(Connection connection) : base(connection)
        {
            // LogicStartQuestCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.QuestData = stream.ReadDataReference<LogicQuestData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            LogicQuest quest = gamemode.Avatar.Quests[this.QuestData.GlobalID];
            quest.Start(gamemode.Avatar, this.QuestData);
        }
    }
}