﻿namespace Supercell.Life.Server.Protocol.Commands.Client
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

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            gamemode.Resigned = true;

            var battle = gamemode.Battle;

            if (battle != null)
            {
                var enemy = battle.GetEnemy(gamemode.Avatar);

                var cmd = new LogicResignCommand(enemy.Connection)
                {
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.EnqueueCommand(this, cmd);

                // battle.Stop();
            }
        }
    }
}
