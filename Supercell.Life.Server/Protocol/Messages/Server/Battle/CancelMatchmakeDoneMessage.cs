namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class CancelMatchmakeDoneMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelMatchmakeDoneMessage"/> class.
        /// </summary>
        public CancelMatchmakeDoneMessage(Connection connection) : base(connection)
        {
            this.Type             = Message.CancelMatchmakeDone;
            this.Connection.State = State.Home;
        }
    }
}