namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicStartMatchmakeCommand : LogicCommand
    {
        internal LogicQuestData Quest;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStartMatchmakeCommand"/> class.
        /// </summary>
        public LogicStartMatchmakeCommand(Connection connection) : base(connection)
        {
            // LogicStartMatchmakeCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Quest = stream.ReadDataReference<LogicQuestData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            LogicGameMode opponent = Matchmaking.Dequeue();
            
            if (opponent != null)
            {
                LogicBattle battle = new LogicBattle(gamemode, opponent)
                {
                    PvPTier = this.Quest
                };

                Battles.Add(battle);

                gamemode.Avatar.GameMode.Battle     = battle;
                opponent.Connection.GameMode.Battle = battle;

                battle.Start();
            }
            else
            {
                new PvpMatchmakeNotificationMessage(this.Connection).Send();
                Matchmaking.Enqueue(gamemode);
            }
        }
    }
}
