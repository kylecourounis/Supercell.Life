namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class ChangeAllianceSettingsMessage : PiranhaMessage
    {
        internal string AllianceDescription;
        internal LogicAllianceBadgeData BadgeData;
        internal int AllianceType;
        internal int TrophyLimit;

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
        /// Initializes a new instance of the <see cref="ChangeAllianceSettingsMessage"/> class.
        /// </summary>
        public ChangeAllianceSettingsMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // ChangeAllianceSettingsMessage.
        }

        internal override void Decode()
        {
            this.AllianceDescription = this.Stream.ReadString();
            this.Stream.ReadString(); // ?
            this.BadgeData           = this.Stream.ReadDataReference<LogicAllianceBadgeData>();
            this.AllianceType        = this.Stream.ReadInt();
            this.TrophyLimit         = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            if (this.Connection.Avatar.IsInAlliance)
            {
                Alliance alliance = Alliances.Get(this.Connection.Avatar.Alliance.Identifier);

                if (alliance != null)
                {
                    this.CheckValues(alliance);
                    Alliances.Save(alliance);

                    foreach (LogicClientAvatar avatar in alliance.Members.Select(member => Avatars.Get(member.Identifier)).Where(avatar => avatar.Connection != null))
                    {
                        new AvailableServerCommandMessage(avatar.Connection, new LogicAllianceSettingsChangedCommand(this.Connection)
                        {
                            Badge = this.BadgeData
                        }).Send();
                    }
                }
            }
        }

        private void CheckValues(Alliance alliance)
        {
            if (alliance != null)
            {
                if (!string.IsNullOrEmpty(this.AllianceDescription))
                {
                    alliance.Description = this.AllianceDescription;
                }

                if (this.BadgeData.GlobalID != alliance.Badge)
                {
                    alliance.Badge = this.BadgeData.GlobalID;
                }

                if (alliance.Type != (Hiring)this.AllianceType)
                {
                    alliance.Type = (Hiring)this.AllianceType;
                }

                if (alliance.RequiredTrophies != this.TrophyLimit)
                {
                    alliance.RequiredTrophies = this.TrophyLimit;
                }
            }
        }
    }
}