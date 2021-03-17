namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System.Linq;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AllianceDataMessage : PiranhaMessage
    {
        private readonly Alliance Alliance;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Alliance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataMessage"/> class.
        /// </summary>
        internal AllianceDataMessage(Connection connection, Alliance alliance) : base(connection)
        {
            this.Type     = Message.AllianceData;
            this.Alliance = alliance;
        }

        internal override void Encode()
        {
            this.Alliance.Encode(this.Stream);

            this.Stream.WriteInt(this.Alliance.Members.Size);
            foreach (AllianceMember member in this.Alliance.Members.OrderByDescending(avatar => avatar.Score))
            {
                member.Encode(this.Stream);
            }

            this.Stream.WriteInt(this.Alliance.TeamGoalTimer.Timer.RemainingSecs);
        }
    }
}
