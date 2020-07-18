namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class PvpMatchmakeNotificationMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PvpMatchmakeNotificationMessage"/> class.
        /// </summary>
        public PvpMatchmakeNotificationMessage(Connection connection) : base(connection)
        {
            this.Type = Message.PvpMatchmakeNotification;
        }

        internal override void Encode()
        {
            this.Stream.WriteString(string.Empty);
        }
    }
}