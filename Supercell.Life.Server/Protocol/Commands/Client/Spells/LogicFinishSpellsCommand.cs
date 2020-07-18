namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicFinishSpellsCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicFinishSpellsCommand"/> class.
        /// </summary>
        public LogicFinishSpellsCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicFinishSpellsCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute()
        {
            int speedUpCost = LogicGamePlayUtil.GetSpeedUpCost(this.Connection.Avatar.SpellTimer.Timer.RemainingSecs, LogicGamePlayUtil.GetSpeedUpCostMultiplier(2));

            if (this.Connection.Avatar.Diamonds >= speedUpCost)
            {
                this.Connection.Avatar.Diamonds -= speedUpCost;
                this.Connection.Avatar.SpellTimer.Finish();
            }

            this.Connection.Avatar.Save();
        }
    }
}