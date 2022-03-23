namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicBuyResourceCommand : LogicCommand
    {
        internal CommodityType Resource;
        internal int Amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyResourceCommand"/> class.
        /// </summary>
        public LogicBuyResourceCommand(Connection connection) : base(connection)
        {
            // LogicBuyResourceCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Resource = (CommodityType)stream.ReadInt();
            this.Amount   = stream.ReadInt();

            stream.ReadByte();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            switch (this.Resource)
            {
                case CommodityType.Gold:
                {
                    int cost = LogicGamePlayUtil.GetResourceDiamondCost(this.Amount, CommodityType.Gold);
                    
                    if (gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost))
                    {
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, this.Amount);
                    }

                    break;
                }
                case CommodityType.Energy:
                {
                    int cost = LogicGamePlayUtil.GetResourceDiamondCost(this.Amount, CommodityType.Energy);
                    
                    if (gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost))
                    {
                        gamemode.Avatar.EnergyTimer.Stop();
                        gamemode.Avatar.SetCommodityCount(CommodityType.Energy, gamemode.Avatar.MaxEnergy);
                    }

                    break;
                }
            }
        }
    }
}
