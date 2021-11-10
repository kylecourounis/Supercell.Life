namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicStopSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;
        internal int Slot;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStopSpellCommand"/> class.
        /// </summary>
        public LogicStopSpellCommand(Connection connection) : base(connection)
        {
            // LogicStopSpellCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Spell = stream.ReadDataReference<LogicSpellData>();
            this.Slot  = stream.ReadInt();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            gamemode.Avatar.SpellTimer.Reset(this.Spell);
        }
    }
}