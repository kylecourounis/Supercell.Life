namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Alliance.Entries;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;
    using Supercell.Life.Server.Protocol.Commands.Server;

    internal class LeaveAllianceMessage : PiranhaMessage
    {
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
        /// Initializes a new instance of the <see cref="LeaveAllianceMessage"/> class.
        /// </summary>
        public LeaveAllianceMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LeaveAllianceMessage.
        }

        internal override void Handle()
        {
            LogicClientAvatar avatar = this.Connection.GameMode.Avatar;
            Alliance alliance        = avatar.Alliance;

            if (alliance != null)
            {
                AllianceMember member = alliance.Members.Find(m => m.Identifier == avatar.Identifier);
                
                avatar.ClanHighID = 0;
                avatar.ClanLowID  = 0;

                avatar.Save();

                if (alliance.Members.Size == 1)
                {
                    Alliances.Delete(alliance);
                }
                else
                {
                    if (member.Role == (int)Alliance.Role.Leader)
                    {
                        LeaveAllianceMessage.FindNewLeader(alliance, member);
                    }
                }

                new AvailableServerCommandMessage(this.Connection, new LogicLeaveAllianceCommand(this.Connection, alliance.Identifier)).Send();
                alliance.AddEntry(new AllianceEventStreamEntry(member, member, AllianceStreamEntry.AllianceStreamEvent.Left));

                alliance.Members.Remove(member);

                alliance.Save();
            }
        }

        private static void FindNewLeader(Alliance alliance, AllianceMember member)
        {
            LogicClientAvatar avatar    = Avatars.Get(member.Identifier);

            AllianceMember newLeader    = alliance.Members.Find(member => member.Role == (int)Alliance.Role.CoLeader);
            const Alliance.Role newRole = Alliance.Role.Leader;

            if (newLeader != null)
            {
                newLeader.Role = (int)newRole;
            }
            else
            {
                newLeader = alliance.Members.Find(member => member.Role == (int)Alliance.Role.Admin);

                if (newLeader != null)
                {
                    newLeader.Role = (int)newRole;
                }
                else
                {
                    newLeader = alliance.Members[Loader.Random.Rand(alliance.Members.Count)];

                    if (newLeader != null)
                    {
                        newLeader.Role = (int)newRole;
                    }
                }
            }

            alliance.AddEntry(new AllianceEventStreamEntry(member, member, AllianceStreamEntry.AllianceStreamEvent.Demoted));
            alliance.AddEntry(new AllianceEventStreamEntry(newLeader, member, AllianceStreamEntry.AllianceStreamEvent.Promoted));

            avatar.Save();
            Avatars.Get(newLeader.Identifier).Save();

            new AvailableServerCommandMessage(avatar.Connection, new LogicChangeAllianceRoleCommand(avatar.Connection)
            {
                Role = newRole
            }).Send();
        }
    }
}