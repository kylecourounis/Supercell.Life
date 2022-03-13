namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeItemCommand : LogicCommand
    {
        internal LogicItemsData Item;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeItemCommand"/> class.
        /// </summary>
        public LogicUpgradeItemCommand(Connection connection) : base(connection)
        {
            // LogicUpgradeItemCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Item = stream.ReadDataReference<LogicItemsData>();
            
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.ExpLevel >= this.Item.RequiredXp)
            {
                string[] cost = this.Item.Cost[gamemode.Avatar.ItemLevels.GetCount(this.Item.GlobalID) + 1].Split(',');

                if (cost.Length >= 7)
                {
                    int diamonds = LogicStringUtil.ConvertToInt(cost[0]);
                    int gold     = LogicStringUtil.ConvertToInt(cost[1]);
                    int energy   = LogicStringUtil.ConvertToInt(cost[2]);
                    int orb1     = LogicStringUtil.ConvertToInt(cost[3]);
                    int orb2     = LogicStringUtil.ConvertToInt(cost[4]);
                    int orb3     = LogicStringUtil.ConvertToInt(cost[5]);
                    int orb4     = LogicStringUtil.ConvertToInt(cost[6]);

                    if (diamonds != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -diamonds))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough diamonds. (Diamonds : {gamemode.Avatar.Diamonds}, Require : {diamonds})");
                            return;
                        }
                    }

                    if (gold != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -gold))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough gold. (Gold : {gamemode.Avatar.Gold}, Require : {gold})");
                            return;
                        }
                    }

                    if (energy != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Energy, -energy))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough energy. (Energy : {gamemode.Avatar.Energy}, Require : {energy}.");
                            return;
                        }
                    }

                    if (orb1 != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb1, -orb1))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb1. (Orb1 : {gamemode.Avatar.Orb1}, Require : {orb1})");
                            return;
                        }
                    }

                    if (orb2 != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb2, -orb2))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb2. (Orb2 : {gamemode.Avatar.Orb2}, Require : {orb2})");
                            return;
                        }
                    }

                    if (orb3 != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb3, -orb3))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb3. (Orb3 : {gamemode.Avatar.Orb3}, Require : {orb3})");
                            return;
                        }
                    }

                    if (orb4 != 0)
                    {
                        if (!gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb4, -orb4))
                        {
                            Debugger.Error($"Unable to upgrade the item. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb4. (Orb4 : {gamemode.Avatar.Orb4}, Require : {orb4})");
                            return;
                        }
                    }
                }

                gamemode.Avatar.ItemInventories.AddItem(this.Item.GlobalID, 1);
                gamemode.Avatar.ItemLevels.AddItem(this.Item.GlobalID, 1);
            }
        }
    }
}