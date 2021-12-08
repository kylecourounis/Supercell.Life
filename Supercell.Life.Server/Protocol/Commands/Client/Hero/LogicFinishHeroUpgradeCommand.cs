namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicFinishHeroUpgradeCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicFinishHeroUpgradeCommand"/> class.
        /// </summary>
        public LogicFinishHeroUpgradeCommand(Connection connection) : base(connection)
        {
            // LogicFinishHeroUpgradeCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            int cost = LogicGamePlayUtil.GetSpeedUpCost(gamemode.Avatar.HeroUpgrade.Timer.RemainingSecs, 0);

            if (gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost))
            {
                gamemode.Avatar.HeroUpgrade.Finish();
            }
        }
    }
}
