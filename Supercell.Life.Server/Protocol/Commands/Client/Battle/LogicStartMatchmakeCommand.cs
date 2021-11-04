namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicStartMatchmakeCommand : LogicCommand
    {
        internal LogicQuestData Quest;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartMatchmakeCommand"/> class.
        /// </summary>
        public LogicStartMatchmakeCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicStartMatchmakeCommand.
        }

        internal override void Decode()
        {
            this.Quest = this.Stream.ReadDataReference<LogicQuestData>();

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            LogicClientAvatar opponent = Waiting.Dequeue();
            
            if (opponent != null)
            {
                LogicBattle battle = new LogicBattle(gamemode.Avatar, opponent)
                {
                    PvPTier = this.Quest
                };

                Battles.Add(battle);

                gamemode.Avatar.Battle = battle;
                opponent.Battle        = battle;

                battle.Start();
            }
            else
            {
                new PvpMatchmakeNotificationMessage(this.Connection).Send();
                Waiting.Enqueue(gamemode.Avatar);
            }
        }
    }
}
