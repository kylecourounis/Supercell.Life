namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicFinishSpellsCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicFinishSpellsCommand"/> class.
        /// </summary>
        public LogicFinishSpellsCommand(Connection connection) : base(connection)
        {
            // LogicFinishSpellsCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            int cost = LogicGamePlayUtil.GetSpeedUpCost(gamemode.Avatar.SpellTimer.Timer.RemainingSecs, 2);

            if (gamemode.Avatar.Diamonds >= cost)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost);
                gamemode.Avatar.SpellTimer.Finish();
            }
        }
    }
}