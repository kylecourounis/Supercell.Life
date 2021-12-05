namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Logic.Game.Objects;
    using Supercell.Life.Server.Network;

    internal class LogicStartSailingCommand : LogicCommand
    {
        internal LogicArrayList<LogicHeroData> Heroes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartSailingCommand"/> class.
        /// </summary>
        public LogicStartSailingCommand(Connection connection) : base(connection)
        {
            this.Heroes = new LogicArrayList<LogicHeroData>();
        }

        internal override void Decode(ByteStream stream)
        {
            for (int i = 0; i < 3; i++)
            {
                LogicHeroData hero = stream.ReadDataReference<LogicHeroData>();

                if (hero != null)
                {
                    this.Heroes.Add(hero);
                }
            }

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            foreach (var hero in this.Heroes)
            {
                gamemode.Avatar.Sailing.Heroes.AddItem(hero.GlobalID, 1);

                int characterIdx = LogicCharacters.GetIndex(hero);

                int level = gamemode.Avatar.HeroLevels.GetCount(hero.GlobalID);

                if (level == 0)
                {
                    level = characterIdx;
                }
                else
                {
                    level += characterIdx;

                    if (level > Globals.ShipGoldPerHeroLevel.Count)
                    {
                        level = Globals.ShipGoldPerHeroLevel.Count;
                    }
                }
                
                gamemode.Avatar.Sailing.HeroLevels.AddItem(hero.GlobalID, level);
            }

            gamemode.Avatar.Sailing.Start();
            
            gamemode.Avatar.Variables.AddItem(LogicVariables.SailCount.GlobalID, 1);
        }
    }
}

