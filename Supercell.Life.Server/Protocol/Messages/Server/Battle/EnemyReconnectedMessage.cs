namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class EnemyReconnectedMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyReconnectedMessage"/> class.
        /// </summary>
        public EnemyReconnectedMessage(Connection connection) : base(connection)
        {
            this.Type = Message.EnemyReconnected;
        }
    }
}
