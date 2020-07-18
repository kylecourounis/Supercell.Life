namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorStateMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorStateMessage"/> class.
        /// </summary>
        public SectorStateMessage(Connection connection) : base(connection)
        {
            this.Type             = Message.SectorState;
            this.Connection.State = State.Battle;
        }

        internal override void Encode()
        {
            //this.Connection.Avatar.Encode(this.Stream);
            //this.Battle.Avatars.Find(val => val.Identifier != this.Connection.Avatar.Identifier).Encode(this.Stream);

            foreach (LogicClientAvatar avatar in this.Battle.Avatars)
            {
                avatar.Encode(this.Stream);
            }

            this.Battle.Encode(this.Stream);

            //this.Stream.WriteInt(0);
            //this.Stream.WriteInt(0);
            //this.Stream.WriteInt(0);
            //this.Stream.WriteInt(0);

            //this.Stream.WriteBoolean(true);

            //this.Stream.WriteDataReference(this.Battle.PvPTier);
            //this.Stream.WriteDataReference((LogicObstacleData)CSV.Tables.Get(Gamefile.Obstacles).GetDataByName("Bush_A"));
        }
    }
}
