namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System;

    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class OwnAvatarDataMessage : PiranhaMessage
    {
        private readonly int TimeSinceLastSave;

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
            this.Type              = Message.OwnAvatarData;
            this.TimeSinceLastSave = (int)DateTime.UtcNow.Subtract(this.Connection.Avatar.Update).TotalSeconds;
            this.Connection.State  = State.Home;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.TimeSinceLastSave);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);

            this.Connection.Avatar.Encode(this.Stream);
        }

        internal override void Handle()
        {
            this.Connection.Avatar.GameMode.AdjustSubTick();
            this.Connection.Avatar.GameMode.FastForward(this.TimeSinceLastSave);
            this.Connection.Avatar.GameMode.Tick();
        }
    }
}
