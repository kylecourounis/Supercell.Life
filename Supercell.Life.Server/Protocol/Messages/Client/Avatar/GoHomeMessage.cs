namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
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
            this.ShowValues();

            if (this.Connection.Avatar.Resigned)
            {
                this.Connection.Avatar.LoseBattle();
            }
            else
            {
                if (!this.Connection.Avatar.Battle.Stopped)
                {
                    this.Connection.Avatar.Battle.Stop();
                }

                this.GetBattleResult();
            }
             
            this.Connection.Avatar.Resigned = false;

            new OwnAvatarDataMessage(this.Connection).Send();
        }

        private void GetBattleResult()
        {
            if (this.Connection.Avatar.OngoingQuestData != null)
            {
                if (this.Connection.Avatar.OngoingQuestData.GlobalID == 6000015)
                {
                    this.Connection.Avatar.OngoingQuestData.Save();
                }
            }
            else
            {
                // this.Connection.Avatar.WinBattle();
            }
        }
    }
}
