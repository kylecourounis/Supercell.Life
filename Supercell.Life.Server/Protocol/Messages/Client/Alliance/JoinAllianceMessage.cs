namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Alliance.Streams;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class JoinAllianceMessage : PiranhaMessage
    {
        internal LogicLong AllianceID;

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
        /// Initializes a new instance of the <see cref="JoinAllianceMessage"/> class.
        /// </summary>
        public JoinAllianceMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // JoinAllianceMessage.
        }

        internal override void Decode()
        {
            this.AllianceID = this.Stream.ReadLogicLong();
        }

        internal override void Handle()
        {
            Alliance alliance = Alliances.Get(this.AllianceID);

            if (alliance != null)
            {
                if (!alliance.Members.Contains(alliance.Members.Find(member => member.Identifier == this.Connection.Avatar.Identifier)))
                {
                    AllianceMember member = new AllianceMember(this.Connection.Avatar, Alliance.Role.Member);
                    alliance.Members.Add(member);

                    this.Connection.Avatar.ClanHighID = alliance.HighID;
                    this.Connection.Avatar.ClanLowID  = alliance.LowID;

                    this.Connection.Avatar.Save();

                    Alliances.Save(alliance);

                    new AvailableServerCommandMessage(this.Connection, new LogicChangeAllianceRoleCommand(this.Connection)
                    {
                        Role = Alliance.Role.Member
                    }).Send();

                    new AvailableServerCommandMessage(this.Connection, new LogicJoinAllianceCommand(this.Connection)).Send();

                    new AllianceStreamMessage(this.Connection).Send();
                    alliance.AddEntry(new StreamEntry(member, member, StreamEntry.StreamEvent.Joined));

                    this.Connection.Avatar.Save();
                }
            }
        }
    }
}