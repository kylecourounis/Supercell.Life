namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
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
            var battle = gamemode.Battle;

            if (battle != null)
            {
                battle.ResetTurn(gamemode.Avatar);

                var cmd = new LogicMultiplayerTurnTimedOutCommand(battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier).Connection)
                {
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.EnqueueCommand(this, cmd);
            }
        }
    }
}
