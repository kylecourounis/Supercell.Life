namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicClaimAchievementCommand : LogicCommand
    {
        private LogicAchievementData Achievement;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicClaimAchievementCommand"/> class.
        /// </summary>
        public LogicClaimAchievementCommand(Connection connection) : base(connection)
        {
            // LogicClaimAchievementCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Achievement = stream.ReadDataReference<LogicAchievementData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (this.Achievement != null && !gamemode.Avatar.AchievementProgress.ContainsKey(this.Achievement.GlobalID))
            {
                gamemode.Avatar.AchievementProgress.AddItem(this.Achievement.GlobalID, this.Achievement.ActionCount);

                gamemode.Avatar.CommodityChangeCountHelper(LogicCommodityType.Experience, this.Achievement.ExpReward);
                gamemode.Avatar.CommodityChangeCountHelper(LogicCommodityType.FreeDiamonds, this.Achievement.DiamondReward);
            }
            else
            {
                Debugger.Error($"Achievement with GlobalID: {this.Achievement.GlobalID} is already in the achievements list!");
            }
        }
    }
}
