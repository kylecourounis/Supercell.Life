namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Titan.Logic;

    internal class LogicStartSailingCommand : LogicCommand
    {
        internal LogicArrayList<LogicHeroData> Heroes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartSailingCommand"/> class.
        /// </summary>
        public LogicStartSailingCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Heroes = new LogicArrayList<LogicHeroData>();
        }

        internal override void Decode()
        {
            for (int i = 0; i < 3; i++)
            {
                LogicHeroData hero = this.Stream.ReadDataReference<LogicHeroData>();

                if (hero != null)
                {
                    this.Heroes.Add(hero);
                }
            }

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            foreach (var hero in this.Heroes)
            {
                gamemode.Avatar.Sailing.Heroes.AddItem(hero.GlobalID, 1);
                gamemode.Avatar.Sailing.HeroLevels.AddItem(hero.GlobalID, gamemode.Avatar.HeroLevels.GetCount(hero.GlobalID));
            }

            gamemode.Avatar.Sailing.Start();
            
            gamemode.Avatar.Variables.AddItem(LogicVariables.SailCount.GlobalID, 1);
        }
    }
}

