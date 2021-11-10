namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;

    internal class LogicKickAllianceMemberCommand : LogicCommand
    {
        internal LogicLong Identifier;
        internal string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicKickAllianceMemberCommand"/> class.
        /// </summary>
        public LogicKickAllianceMemberCommand(Connection connection) : base(connection)
        {
            // LogicKickAllianceMemberCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Identifier = stream.ReadLogicLong();
            // this.Name       = stream.ReadString();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
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