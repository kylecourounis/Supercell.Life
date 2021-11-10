namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicModifyTeamCommand : LogicCommand
    {
        private readonly LogicArrayList<LogicHeroData> Heroes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicModifyTeamCommand"/> class.
        /// </summary>
        public LogicModifyTeamCommand(Connection connection) : base(connection)
        {
            this.Heroes = new LogicArrayList<LogicHeroData>();
        }

        internal override void Decode(ByteStream stream)
        {
            this.Heroes.Add(stream.ReadDataReference<LogicHeroData>());
            this.Heroes.Add(stream.ReadDataReference<LogicHeroData>());
            this.Heroes.Add(stream.ReadDataReference<LogicHeroData>());

            stream.ReadInt(); // -1 (not sure what this is for)

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.Team.Count > 3)
            {
                gamemode.Avatar.Team.Clear();
            }

            for (int i = 0; i < this.Heroes.Count; i++)
            {
                gamemode.Avatar.Team[i] = this.Heroes[i].GlobalID;
            }
        }
    }
}
