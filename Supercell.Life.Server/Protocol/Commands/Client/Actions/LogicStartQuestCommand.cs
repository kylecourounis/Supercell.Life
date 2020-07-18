namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;
    using Supercell.Life.Server.Network;

    internal class LogicStartQuestCommand : LogicCommand
    {
        internal LogicQuestData QuestData;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartQuestCommand"/> class.
        /// </summary>
        public LogicStartQuestCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicStartQuestCommand.
        }

        internal override void Decode()
        {
            this.QuestData = this.Stream.ReadDataReference<LogicQuestData>();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            LogicQuest quest = LogicQuests.Quests[this.QuestData.GlobalID];
            quest.Avatar     = this.Connection.Avatar;
            quest.Data       = this.QuestData;

            quest.Start();

            this.Connection.Avatar.Save();
        }
    }
}