namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Entries;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicSendAllianceMailCommand : LogicCommand
    {
        internal string Title;
        internal string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSendAllianceMailCommand"/> class.
        /// </summary>
        public LogicSendAllianceMailCommand(Connection connection) : base(connection)
        {
            // LogicSendAllianceMailCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Message = stream.ReadString();
            this.Title   = stream.ReadString();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            foreach (var member in gamemode.Avatar.Alliance.Members)
            {
                var avatar = Avatars.Get(member.Identifier);

                var mail = new AllianceMailAvatarStreamEntry(avatar, gamemode.Avatar)
                {
                    AllianceHighID = gamemode.Avatar.Alliance.HighID,
                    AllianceLowID  = gamemode.Avatar.Alliance.LowID,
                    AllianceName   = gamemode.Avatar.Alliance.Name,
                    AllianceBadge  = gamemode.Avatar.Alliance.Badge,
                    Title          = this.Title,
                    Message        = this.Message
                };

                avatar.StreamEntries.Add(mail);

                if (avatar.Connection != null)
                {
                    new AvatarStreamEntryMessage(avatar.Connection, mail).Send();
                }
            }

            gamemode.Avatar.TeamMailCooldownTimer.Start();
        }
    }
}