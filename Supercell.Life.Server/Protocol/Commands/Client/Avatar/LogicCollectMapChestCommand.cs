namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Game.Objects;
    using Supercell.Life.Server.Network;

    internal class LogicCollectMapChestCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCollectMapChestCommand"/> class.
        /// </summary>
        public LogicCollectMapChestCommand(Connection connection) : base(connection)
        {
            // LogicCollectMapChestCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.MapChestsOpened == 3)
            {
                gamemode.MapChestsOpened = 0;
            }

            gamemode.MapChestsOpened++;

            LogicChest chest = new LogicChest(gamemode.Avatar);
            chest.CreateMapChest();
        }
    }
}
