namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicReturnToMapCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicReturnToMapCommand"/> class.
        /// </summary>
        public LogicReturnToMapCommand(Connection connection) : base(connection)
        {
            // LogicReturnToMapCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            // We need to figure out the quest completion system at some point.

            if (gamemode.Avatar.OngoingQuestData != null)
            {
                if (gamemode.Avatar.ExpLevel >= gamemode.Avatar.OngoingQuestData.Data.RequiredXpLevel)
                {
                    gamemode.Avatar.OngoingQuestData.Save();
                }
                else
                {
                    Debugger.Warning("Player does not have the required XP level => not saving the level.");
                }
            }
        }
    }
}