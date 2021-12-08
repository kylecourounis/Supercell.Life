namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class CreateAllianceMessage : PiranhaMessage
    {
        internal string AllianceName;
        internal string AllianceDescription;
        internal LogicAllianceBadgeData BadgeData;
        internal int Type;
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
        /// Initializes a new instance of the <see cref="CreateAllianceMessage"/> class.
        /// </summary>
        public CreateAllianceMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // CreateAllianceMessage.
        }

        internal override void Decode()
        {
            this.AllianceName        = this.Stream.ReadString();
            this.AllianceDescription = this.Stream.ReadString();
            this.BadgeData           = this.Stream.ReadDataReference<LogicAllianceBadgeData>();
            this.Type                = this.Stream.ReadInt();
            this.TrophyLimit         = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            if (!this.Connection.GameMode.Avatar.IsInAlliance && this.Connection.GameMode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -Globals.AllianceCreateCost))
            {
                Alliance alliance = Alliances.Create();

                if (alliance != null)
                {
                    alliance.Name = this.AllianceName;
                    alliance.Description = this.AllianceDescription;
                    alliance.Badge = this.BadgeData.GlobalID;
                    alliance.Type = (Hiring)this.Type;
                    alliance.RequiredTrophies = this.TrophyLimit;

                    alliance.Members.Add(new AllianceMember(this.Connection.GameMode.Avatar, Alliance.Role.Leader));

                    Alliances.Save(alliance);

                    this.Connection.GameMode.Avatar.ClanHighID = alliance.HighID;
                    this.Connection.GameMode.Avatar.ClanLowID = alliance.LowID;

                    new AvailableServerCommandMessage(this.Connection, new LogicChangeAllianceRoleCommand(this.Connection)
                    {
                        Role = Alliance.Role.Leader
                    }).Send();

                    new AvailableServerCommandMessage(this.Connection, new LogicJoinAllianceCommand(this.Connection)
                    {
                        Alliance = alliance,
                        JustCreated = true
                    }).Send();
                }

                this.Connection.GameMode.Avatar.Save();
            }
        }
    }
}