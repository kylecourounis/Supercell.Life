namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeHeroCommand : LogicCommand
    {
        private LogicHeroData Hero;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeHeroCommand"/> class.
        /// </summary>
        public LogicUpgradeHeroCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicUpgradeHeroCommand.
        }

        internal override void Decode()
        {
            this.Hero = this.Stream.ReadDataReference<LogicHeroData>();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (this.Hero != null)
            {
                if (this.Connection.Avatar.HeroUpgrade.CanUpgrade(this.Hero))
                {
                    if (this.Connection.Avatar.ExpLevel < this.Hero.RequiredXpLevel)
                    {
                        Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) is not at the required level. (Level : {this.Connection.Avatar.ExpLevel}, Require : {this.Hero.RequiredXpLevel})");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.Hero.RequiredQuest))
                    {
                        if (!this.Connection.Avatar.NpcProgress.ContainsKey(this.Hero.RequiredQuestData.GlobalID))
                        {
                            Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) has not unlocked the required quest.");
                            return;
                        }
                    }

                    string[] cost = this.Hero.Cost[this.Connection.Avatar.HeroLevels.GetCount(this.Hero.GlobalID)].Split(',');

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
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough diamonds. (Diamonds : {this.Connection.Avatar.Diamonds}, Require : {diamonds})");
                                return;
                            }
                        }

                        if (gold != 0)
                        {
                            if (this.Connection.Avatar.Gold < gold)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough gold. (Gold : {this.Connection.Avatar.Gold}, Require : {gold})");
                                return;
                            }
                        }

                        if (energy != 0)
                        {
                            if (this.Connection.Avatar.Energy < energy)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough energy. (Energy : {this.Connection.Avatar.Energy}, Require : {energy}.");
                                return;
                            }
                        }
                        
                        if (orb1 != 0)
                        {
                            if (this.Connection.Avatar.Orb1 < orb1)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb1. (Orb1 : {this.Connection.Avatar.Orb1}, Require : {orb1})");
                                return;
                            }
                        }

                        if (orb2 != 0)
                        {
                            if (this.Connection.Avatar.Orb2 < orb2)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb2. (Orb2 : {this.Connection.Avatar.Orb2}, Require : {orb2})");
                                return;
                            }
                        }

                        if (orb3 != 0)
                        {
                            if (this.Connection.Avatar.Orb3 < orb3)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb3. (Orb3 : {this.Connection.Avatar.Orb3}, Require : {orb3})");
                                return;
                            }
                        }

                        if (orb4 != 0)
                        {
                            if (this.Connection.Avatar.Orb4 < orb4)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {this.Connection.Avatar.Name} ({this.Connection.Avatar}) does not have enough of orb4. (Orb4 : {this.Connection.Avatar.Orb4}, Require : {orb4})");
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

                    this.Connection.Avatar.HeroUpgrade.Start(this.Hero);

                    this.Connection.Avatar.Save();
                }
            }
        }
    }
}