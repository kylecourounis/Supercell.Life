namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeItemCommand : LogicCommand
    {
        private LogicItemsData Item;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeItemCommand"/> class.
        /// </summary>
        public LogicUpgradeItemCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicUpgradeItemCommand.
        }

        internal override void Decode()
        {
            this.Item = this.Stream.ReadDataReference<LogicItemsData>();
            
            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (this.Connection.Avatar.ExpLevel >= this.Item.RequiredXp)
            {
                string[] cost = this.Item.Cost[this.Connection.Avatar.ItemLevels.GetCount(this.Item.GlobalID)].Split(',');

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
                        if (this.Connection.Avatar.Diamonds < diamonds)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough diamonds. (Diamonds : {this.Connection.Avatar.Diamonds}, Require : {diamonds})");
                            return;
                        }
                    }

                    if (gold != 0)
                    {
                        if (this.Connection.Avatar.Gold < gold)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough gold. (Gold : {this.Connection.Avatar.Gold}, Require : {gold})");
                            return;
                        }
                    }

                    if (energy != 0)
                    {
                        if (this.Connection.Avatar.Energy < energy)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough energy. (Energy : {this.Connection.Avatar.Energy}, Require : {energy}.");
                            return;
                        }
                    }

                    if (orb1 != 0)
                    {
                        if (this.Connection.Avatar.Orb1 < orb1)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb1. (Orb1 : {this.Connection.Avatar.Orb1}, Require : {orb1})");
                            return;
                        }
                    }

                    if (orb2 != 0)
                    {
                        if (this.Connection.Avatar.Orb2 < orb2)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb2. (Orb2 : {this.Connection.Avatar.Orb2}, Require : {orb2})");
                            return;
                        }
                    }

                    if (orb3 != 0)
                    {
                        if (this.Connection.Avatar.Orb3 < orb3)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb3. (Orb3 : {this.Connection.Avatar.Orb3}, Require : {orb3})");
                            return;
                        }
                    }

                    if (orb4 != 0)
                    {
                        if (this.Connection.Avatar.Orb4 < orb4)
                        {
                            Debugger.Error($"Unable to upgrade the item. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb4. (Orb4 : {this.Connection.Avatar.Orb4}, Require : {orb4})");
                            return;
                        }
                    }
                    
                    this.Connection.Avatar.Diamonds -= diamonds;
                    this.Connection.Avatar.Gold     -= gold;
                    this.Connection.Avatar.Energy   -= energy;
                    this.Connection.Avatar.Orb1     -= orb1;
                    this.Connection.Avatar.Orb2     -= orb2;
                    this.Connection.Avatar.Orb3     -= orb3;
                    this.Connection.Avatar.Orb4     -= orb4;
                }

                this.Connection.Avatar.ItemInventories.AddItem(this.Item.GlobalID, 1);
                this.Connection.Avatar.ItemLevels.AddItem(this.Item.GlobalID, 1);
            }

            this.Connection.Avatar.Save();
        }
    }
}