namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicResignCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicResignCommand"/> class.
        /// </summary>
        public LogicResignCommand(Connection connection) : base(connection)
        {
            this.Type = Command.Resign;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicResignCommand"/> class.
        /// </summary>
        public LogicResignCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicResignCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Encode()
        {
            this.WriteHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            gamemode.Avatar.Resigned = true;

            var battle = gamemode.Avatar.Battle;

            if (battle != null)
            {
                LogicResignCommand cmd = new LogicResignCommand(battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier).Connection);

                battle.GetOwnQueue(gamemode.Avatar).Enqueue(this);
                battle.GetEnemyQueue(gamemode.Avatar).Enqueue(cmd);

                battle.Stop();
            }
        }
    }
}
