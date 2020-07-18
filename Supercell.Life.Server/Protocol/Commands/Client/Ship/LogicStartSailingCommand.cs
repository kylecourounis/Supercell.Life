namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicStartSailingCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartSailingCommand"/> class.
        /// </summary>
        public LogicStartSailingCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicStartSailingCommand.
        }

        internal override void Decode()
        {
            for (int i = 0; i < 3; i++)
            {
                LogicHeroData hero = this.Stream.ReadDataReference<LogicHeroData>();

                if (hero != null)
                {
                    this.Connection.Avatar.Sailing.Heroes.AddItem(hero.GlobalID, 1);
                    this.Connection.Avatar.Sailing.HeroLevels.AddItem(hero.GlobalID, this.Connection.Avatar.HeroLevels.GetCount(hero.GlobalID));
                }
            }

            this.ReadHeader();
        }

        internal override void Execute()
        {
            this.Connection.Avatar.Sailing.Start();
            
            this.Connection.Avatar.Variables.AddItem(LogicVariables.SailCount.GlobalID, 1);

            this.Connection.Avatar.Save();
        }
    }
}

