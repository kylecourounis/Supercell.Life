namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class EnemyDisconnectedMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyDisconnectedMessage"/> class.
        /// </summary>
        public EnemyDisconnectedMessage(Connection connection) : base(connection)
        {
            this.Type = Message.EnemyDisconnected;
        }
    }
}
