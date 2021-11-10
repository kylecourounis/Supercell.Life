namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicBuyHeroCommand : LogicCommand
    {
        internal LogicHeroData Hero;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyHeroCommand"/> class.
        /// </summary>
        public LogicBuyHeroCommand(Connection connection) : base(connection)
        {
            // LogicBuyHeroCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Hero = stream.ReadDataReference<LogicHeroData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            gamemode.Avatar.HeroLevels.AddItem(new LogicDataSlot(this.Hero.GlobalID, 0));
        }
    }
}