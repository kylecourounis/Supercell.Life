namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicModifyTeamCommand : LogicCommand
    {
        private readonly LogicArrayList<LogicHeroData> Heroes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicModifyTeamCommand"/> class.
        /// </summary>
        public LogicModifyTeamCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Heroes = new LogicArrayList<LogicHeroData>();
        }

        internal override void Decode()
        {
            this.Heroes.Add(this.Stream.ReadDataReference<LogicHeroData>());
            this.Heroes.Add(this.Stream.ReadDataReference<LogicHeroData>());
            this.Heroes.Add(this.Stream.ReadDataReference<LogicHeroData>());

            this.Stream.ReadInt(); // -1 (not sure what this is for)

            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (this.Connection.Avatar.Team.Count > 3)
            {
                this.Connection.Avatar.Team.Clear();
            }

            for (int i = 0; i < this.Heroes.Count; i++)
            {
                this.Connection.Avatar.Team[i] = this.Heroes[i].GlobalID;
            }

            this.Connection.Avatar.Save();
        }
    }
}
