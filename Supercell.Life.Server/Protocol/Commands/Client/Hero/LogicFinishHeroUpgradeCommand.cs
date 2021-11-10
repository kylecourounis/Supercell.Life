namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicFinishHeroUpgradeCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicFinishHeroUpgradeCommand"/> class.
        /// </summary>
        public LogicFinishHeroUpgradeCommand(Connection connection) : base(connection)
        {
            // LogicFinishHeroUpgradeCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            gamemode.Avatar.HeroUpgrade.Finish();
        }
    }
}
