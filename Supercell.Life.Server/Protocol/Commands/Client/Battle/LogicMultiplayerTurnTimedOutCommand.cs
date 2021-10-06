namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicMultiplayerTurnTimedOutCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMultiplayerTurnTimedOutCommand"/> class.
        /// </summary>
        public LogicMultiplayerTurnTimedOutCommand(Connection connection) : base(connection)
        {
            this.Type = Command.TurnTimedOut;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMultiplayerTurnTimedOutCommand"/> class.
        /// </summary>
        public LogicMultiplayerTurnTimedOutCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicMultiplayerTurnTimedOutCommand.
        }
        
        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Encode()
        {
            this.WriteHeader();
        }

        internal override void Execute()
        {
            var battle = this.Connection.Avatar.GameMode.Battle;

            if (battle != null)
            {
                battle.Reset();

                var cmd = new LogicMultiplayerTurnTimedOutCommand(battle.Avatars.Find(avatar => avatar.Identifier != this.Connection.Avatar.Identifier).Connection)
                {
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.GetOwnQueue(this.Connection.Avatar).Enqueue(this);
                battle.GetEnemyQueue(this.Connection.Avatar).Enqueue(cmd);
            }
        }
    }
}
