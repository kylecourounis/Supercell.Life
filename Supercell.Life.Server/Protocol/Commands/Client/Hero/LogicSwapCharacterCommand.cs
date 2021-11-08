namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicSwapCharacterCommand : LogicCommand
    {
        internal int Unknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSwapCharacterCommand"/> class.
        /// </summary>
        public LogicSwapCharacterCommand(Connection connection) : base(connection)
        {
            this.Type = Command.SwapCharacter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSwapCharacterCommand"/> class.
        /// </summary>
        public LogicSwapCharacterCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicSwapCharacterCommand.
        }

        internal override void Decode()
        {
            this.Unknown = this.Stream.ReadInt();

            this.ReadHeader();
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.Unknown);

            this.WriteHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Avatar.Battle;

            if (battle != null)
            {
                var enemy = battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier);

                var cmd = new LogicSwapCharacterCommand(enemy.Connection)
                {
                    Unknown        = this.Unknown,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.GetOwnQueue(gamemode.Avatar).Enqueue(cmd);
                battle.GetEnemyQueue(gamemode.Avatar).Enqueue(cmd);
            }
        }
    }
}
