namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicKickAllianceMemberCommand : LogicCommand
    {
        internal LogicLong Identifier;
        internal string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicKickAllianceMemberCommand"/> class.
        /// </summary>
        public LogicKickAllianceMemberCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicKickAllianceMemberCommand.
        }

        internal override void Decode()
        {
            this.Identifier = this.Stream.ReadLogicLong();
            // this.Name       = this.Stream.ReadString();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            LogicClientAvatar avatar = Avatars.Get(this.Identifier);

            if (avatar != null)
            {
                Alliance alliance = avatar.Alliance;

                AllianceMember allianceMember = alliance.Members.Find(member => member.Identifier == this.Identifier);

                if (allianceMember != null)
                {
                    alliance.Members.Remove(allianceMember);
                }

                Alliances.Save(alliance);
            }
        }
    }
}