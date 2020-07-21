namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicClaimAchievementCommand : LogicCommand
    {
        private LogicAchievementData Achievement;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicClaimAchievementCommand"/> class.
        /// </summary>
        public LogicClaimAchievementCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicClaimAchievementCommand.
        }

        internal override void Decode()
        {
            this.Achievement = this.Stream.ReadDataReference<LogicAchievementData>();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (this.Achievement != null && !this.Connection.Avatar.AchievementProgress.ContainsKey(this.Achievement.GlobalID))
            {
                this.Connection.Avatar.AchievementProgress.AddItem(this.Achievement.GlobalID, this.Achievement.ActionCount);
                
                this.Connection.Avatar.AddXP(this.Achievement.ExpReward);
                this.Connection.Avatar.AddDiamonds(this.Achievement.DiamondReward, true);

                this.Connection.Avatar.Save();
            }
            else
            {
                Debugger.Error($"Achievement with GlobalID: {this.Achievement.GlobalID} is already in the achievements list!");
            }
        }
    }
}
