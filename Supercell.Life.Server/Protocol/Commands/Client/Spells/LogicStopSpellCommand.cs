namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicStopSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;
        internal int Slot;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicStopSpellCommand"/> class.
        /// </summary>
        public LogicStopSpellCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicStopSpellCommand.
        }

        internal override void Decode()
        {
            this.Spell = this.Stream.ReadDataReference<LogicSpellData>();
            this.Slot  = this.Stream.ReadInt();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            this.Connection.Avatar.SpellTimer.Reset(this.Spell);

            this.Connection.Avatar.Save();
        }
    }
}