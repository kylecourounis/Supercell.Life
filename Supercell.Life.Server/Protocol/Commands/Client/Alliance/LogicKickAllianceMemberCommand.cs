namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Avatar.Entries;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicKickAllianceMemberCommand : LogicCommand
    {
        internal LogicLong Identifier;
        internal string Reason;

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

            if (stream.ReadBoolean())
            {
                this.Reason = stream.ReadString();
            }

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            this.ShowValues();

            LogicClientAvatar avatar = Avatars.Get(this.Identifier);

            if (avatar != null)
            {
                Alliance alliance = avatar.Alliance;

                AllianceMember allianceMember = alliance.Members.Find(member => member.Identifier == this.Identifier);
                AllianceMember executor       = alliance.Members.Find(member => member.Identifier == this.ExecutorID);

                if (allianceMember != null)
                {
                    alliance.Members.Remove(allianceMember);

                    avatar.ClanHighID = 0;
                    avatar.ClanLowID  = 0;
                }

                var kickedMsg = new AllianceKickOutStreamEntry(avatar, gamemode.Avatar)
                {
                    AllianceHighID = gamemode.Avatar.Alliance.HighID,
                    AllianceLowID  = gamemode.Avatar.Alliance.LowID,
                    AllianceName   = gamemode.Avatar.Alliance.Name,
                    AllianceBadge  = gamemode.Avatar.Alliance.Badge,
                    Reason         = this.Reason
                };

                avatar.StreamEntries.Add(kickedMsg);
                new AvatarStreamEntryMessage(avatar.Connection, kickedMsg).Send();

                alliance.AddEntry(new AllianceStreamEntry(allianceMember, executor, AllianceStreamEntry.AllianceStreamEvent.Kick));

                Alliances.Save(alliance);
            }
        }
    }
}