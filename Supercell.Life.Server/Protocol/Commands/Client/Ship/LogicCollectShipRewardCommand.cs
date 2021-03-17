namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicCollectShipRewardCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCollectShipRewardCommand"/> class.
        /// </summary>
        public LogicCollectShipRewardCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicCollectShipRewardCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute()
        {
            this.CalculateLoot();

            this.Connection.Avatar.Variables.Remove(LogicVariables.SailRewardUnclaimed.GlobalID);

            this.Connection.Avatar.Sailing.Heroes.Clear();
            this.Connection.Avatar.Sailing.HeroLevels.Clear();

            this.Connection.Avatar.Save();
        }

        private void CalculateLoot()
        {
            if (CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_GOLD_PER_HERO_LVL") is LogicGlobalData shipGold)
            {
                if (CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_XP_PER_HERO_LVL") is LogicGlobalData shipXp)
                {
                    int gold = 0;
                    int xp = 0;

                    foreach (var level in this.Connection.Avatar.Sailing.HeroLevels.Select(hero => this.Connection.Avatar.HeroLevels.Get(hero.Value.Id)).Select(item => item.Count))
                    {
                        gold += shipGold.NumberArray[level];
                        xp   += shipXp.NumberArray[level];
                    }

                    this.Connection.Avatar.AddGold(gold);
                    this.Connection.Avatar.AddXP(xp);
                }
            }
        }
    }
}
