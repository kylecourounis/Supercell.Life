namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Alliance.Streams;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class ChangeAllianceMemberRoleMessage : PiranhaMessage
    {
        internal LogicLong MemberID;
        internal int Role;

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
        /// Initializes a new instance of the <see cref="ChangeAllianceMemberRoleMessage"/> class.
        /// </summary>
        public ChangeAllianceMemberRoleMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // ChangeAllianceMemberRoleMessage.
        }

        internal override void Decode()
        {
            this.MemberID = this.Stream.ReadLogicLong();
            this.Role     = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            Alliance alliance = Alliances.Get(this.Connection.GameMode.Avatar.Alliance.Identifier);

            if (alliance != null)
            {
                AllianceMember member = alliance.Members.Find(m => m.Identifier == this.MemberID);
                AllianceMember sender = alliance.Members.Find(m => m.Identifier == this.Connection.GameMode.Avatar.Identifier);

                alliance.AddEntry(new StreamEntry(member, sender, ChangeAllianceMemberRoleMessage.IsHigherRoleThan(this.Role, member.Role) ? StreamEntry.StreamEvent.Promoted : StreamEntry.StreamEvent.Demoted));
                member.Role = this.Role;

                if (this.Role == (int)Alliance.Role.Leader)
                {
                    sender.Role = (int)Alliance.Role.CoLeader;
                    alliance.AddEntry(new StreamEntry(sender, sender, StreamEntry.StreamEvent.Demoted));
                }

                Alliances.Save(alliance);

                new AvailableServerCommandMessage(this.Connection, new LogicChangeAllianceRoleCommand(this.Connection)
                {
                    Role = (Alliance.Role)this.Role
                }).Send();
            }
        }

        private static bool IsHigherRoleThan(int role, int toCompare)
        {
            int[] table = { 1, 1, 4, 2, 3 };
            return role >= 5 || toCompare >= 5 || table[toCompare] < table[role];
        }
    }
}