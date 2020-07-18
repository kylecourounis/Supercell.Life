namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicUseSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUseSpellCommand"/> class.
        /// </summary>
        public LogicUseSpellCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicUseSpellCommand.
        }

        internal override void Decode()
        {
            this.Spell = this.Stream.ReadDataReference<LogicSpellData>();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (this.Connection.Avatar.SpellsReady.ContainsKey(this.Spell.GlobalID))
            {
                this.Connection.Avatar.SpellsReady.Remove(this.Spell.GlobalID, 1);
            }

            this.Connection.Avatar.Save();
        }
    }
}