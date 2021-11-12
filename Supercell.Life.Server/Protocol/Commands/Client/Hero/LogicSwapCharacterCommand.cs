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
        
        internal override void Decode(ByteStream stream)
        {
            this.Unknown = stream.ReadInt();

            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.Unknown);
            
            base.Encode(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Battle;

            if (battle != null)
            {
                var enemy = battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier);

                var cmd = new LogicSwapCharacterCommand(enemy.Connection)
                {
                    Unknown        = this.Unknown,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.EnqueueCommand(this, this);
            }
        }
    }
}
