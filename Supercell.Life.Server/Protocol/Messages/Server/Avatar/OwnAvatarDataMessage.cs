namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System;

    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class OwnAvatarDataMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnAvatarDataMessage"/> class.
        /// </summary>
        public OwnAvatarDataMessage(Connection connection) : base (connection)
        {
            this.Type                                         = Message.OwnAvatarData;
            this.Connection.State                             = State.Home;
            this.Connection.GameMode.Avatar.TimeSinceLastSave = (int)DateTime.UtcNow.Subtract(this.Connection.GameMode.Avatar.Update).TotalSeconds;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.EnergyTimer.GetTimeRemaining());
            this.Stream.WriteInt(this.Connection.GameMode.Random.GetIteratedRandomSeed());
            this.Stream.WriteInt(-1);
            this.Stream.WriteInt(-1);

            this.Connection.GameMode.Avatar.Encode(this.Stream);
        }

        internal override void Handle()
        {
            this.Connection.GameMode.AdjustSubTick();
            this.Connection.GameMode.FastForward(this.Connection.GameMode.Avatar.TimeSinceLastSave);
            this.Connection.GameMode.Tick();
        }
    }
}
