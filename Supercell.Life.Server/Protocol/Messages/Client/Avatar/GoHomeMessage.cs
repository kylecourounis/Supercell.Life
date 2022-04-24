namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class GoHomeMessage : PiranhaMessage
    {
        internal bool Value;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Home;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoHomeMessage"/> class.
        /// </summary>
        public GoHomeMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // GoHomeMessage.
        }

        internal override void Decode()
        {
            this.Value = this.Stream.ReadBoolean();
        }

        internal override void Handle()
        {
            if (this.Connection.GameMode.Resigned)
            {
                this.Connection.GameMode.Avatar.LoseBattle();
            }
            else
            {
                if (this.Connection.GameMode.Avatar.OngoingQuestData.GlobalID == 6000015)
                {
                    this.Connection.GameMode.Avatar.OngoingQuestData.Save();
                }
                else
                {
                    if (this.Connection.GameMode.Avatar.DailyMultiplayerTimer.Started)
                    {
                        this.Connection.GameMode.Avatar.CommodityChangeCountHelper(CommodityType.Energy, -4);

                        if (this.Connection.GameMode.Avatar.DailyMultiplayerTimer.BonusPVPAvailable)
                        {
                            this.Connection.GameMode.Avatar.DailyMultiplayerTimer.BonusFreePlayUsed = true;
                        }
                    }
                }
            }
            
            new OwnAvatarDataMessage(this.Connection).Send();
        }
    }
}
