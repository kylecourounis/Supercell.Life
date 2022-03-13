namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Utils;
    
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicBuyRemainingResourcesCommand : LogicCommand
    {
        private int CommandID;
        private LogicCommand Command;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyRemainingResourcesCommand"/> class.
        /// </summary>
        public LogicBuyRemainingResourcesCommand(Connection connection) : base(connection)
        {
            // LogicBuyRemainingResourcesCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            if (stream.ReadBoolean())
            {
                this.CommandID = stream.ReadInt();
                this.Command   = LogicCommandManager.CreateCommand(this.CommandID, this.Connection);

                if (this.Command != null)
                {
                    if (this.Connection.GameMode.CommandManager.IsCommandAllowedInCurrentState(this.Command))
                    {
                        this.Command.Decode(stream);
                    }
                }
            }

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            string[] cost = {};

            switch ((Command)this.CommandID)
            {
                case Enums.Command.BuyHero:
                {
                    var cmd = (LogicBuyHeroCommand)this.Command;
                    cost = cmd.Hero.Cost[gamemode.Avatar.HeroLevels.GetCount(cmd.Hero.GlobalID) + 1].Split(',');

                    break;
                }
                case Enums.Command.UpgradeHero:
                {
                    var cmd = (LogicUpgradeHeroCommand)this.Command;
                    cost = cmd.Hero.Cost[gamemode.Avatar.HeroLevels.GetCount(cmd.Hero.GlobalID) + 1].Split(',');

                    break;
                }
                case Enums.Command.BuyItem:
                {
                    var cmd = (LogicBuyItemCommand)this.Command;
                    cost = cmd.Item.Cost[gamemode.Avatar.ItemLevels.GetCount(cmd.Item.GlobalID) + 1].Split(',');

                    break;
                }
                case Enums.Command.UpgradeItem:
                {
                    var cmd = (LogicUpgradeItemCommand)this.Command;
                    cost = cmd.Item.Cost[gamemode.Avatar.ItemLevels.GetCount(cmd.Item.GlobalID) + 1].Split(',');

                    break;
                }
            }

            if (cost.Length >= 7)
            {
                this.AddResources(cost);
                this.Command.Execute(gamemode);
            }
        }

        private void AddResources(string[] cost)
        {
            LogicGameMode gamemode = this.Connection.GameMode;

            int diamonds = LogicStringUtil.ConvertToInt(cost[0]);
            int gold     = LogicStringUtil.ConvertToInt(cost[1]);
            int energy   = LogicStringUtil.ConvertToInt(cost[2]);
            int orb1     = LogicStringUtil.ConvertToInt(cost[3]);
            int orb2     = LogicStringUtil.ConvertToInt(cost[4]);
            int orb3     = LogicStringUtil.ConvertToInt(cost[5]);
            int orb4     = LogicStringUtil.ConvertToInt(cost[6]);

            int goldChange = gold - gamemode.Avatar.Gold;
            int orb1Change = orb1 - gamemode.Avatar.Orb1;
            int orb2Change = orb2 - gamemode.Avatar.Orb2;
            int orb3Change = orb3 - gamemode.Avatar.Orb3;
            int orb4Change = orb4 - gamemode.Avatar.Orb4;
            
            if (gold != 0)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, goldChange);
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -LogicGamePlayUtil.GetResourceDiamondCost(goldChange, CommodityType.Gold));
            }
            
            if (orb1 != 0)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb1, orb1Change);
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -LogicGamePlayUtil.GetResourceDiamondCost(orb1Change, CommodityType.Orb1));
            }

            if (orb2 != 0)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb2, orb2Change);
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -LogicGamePlayUtil.GetResourceDiamondCost(orb2Change, CommodityType.Orb2));
            }

            if (orb3 != 0)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb3, orb3Change);
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -LogicGamePlayUtil.GetResourceDiamondCost(orb3Change, CommodityType.Orb3));
            }

            if (orb4 != 0)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb4, orb4Change);
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -LogicGamePlayUtil.GetResourceDiamondCost(orb4Change, CommodityType.Orb4));
            }
        }
    }
}