namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicAimCharacterCommand : LogicCommand
    {
        internal int X, Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAimCharacterCommand"/> class.
        /// </summary>
        public LogicAimCharacterCommand(Connection connection) : base(connection)
        {
            this.Type = Command.AimCharacter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAimCharacterCommand"/> class.
        /// </summary>
        public LogicAimCharacterCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicAimCharacterCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();

            this.X = this.Stream.ReadInt();
            this.Y = this.Stream.ReadInt();
        }

        internal override void Encode()
        {
            this.WriteHeader();

            this.Stream.WriteInt(this.X);
            this.Stream.WriteInt(this.Y);
        }

        internal override void Execute()
        {
            var battle = this.Connection.Avatar.Battle;

            if (battle != null)
            {
                LogicAimCharacterCommand cmd = new LogicAimCharacterCommand(battle.Avatars.Find(avatar => avatar.Identifier != this.Connection.Avatar.Identifier).Connection)
                {
                    X = this.X,
                    Y = this.Y
                };

                battle.GetOwnQueue(this.Connection.Avatar).Enqueue(this);
                battle.GetEnemyQueue(this.Connection.Avatar).Enqueue(cmd);
            }
        }
    }
}