namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
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

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            this.X = stream.ReadInt();
            this.Y = stream.ReadInt();
        }

        internal override void Encode(ByteStream stream)
        {
            base.Encode(stream);

            stream.WriteInt(this.X);
            stream.WriteInt(this.Y);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Battle;

            if (battle != null)
            {
                LogicAimCharacterCommand cmd = new LogicAimCharacterCommand(battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier).Connection)
                {
                    X = this.X,
                    Y = this.Y
                };

                battle.GetOwnQueue(gamemode.Avatar).Enqueue(this);
                battle.GetEnemyQueue(gamemode.Avatar).Enqueue(cmd);
            }
        }
    }
}