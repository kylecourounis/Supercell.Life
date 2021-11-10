namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicUseSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUseSpellCommand"/> class.
        /// </summary>
        public LogicUseSpellCommand(Connection connection) : base(connection)
        {
            // LogicUseSpellCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Spell = stream.ReadDataReference<LogicSpellData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.SpellsReady.ContainsKey(this.Spell.GlobalID))
            {
                gamemode.Avatar.SpellsReady.Remove(this.Spell.GlobalID, 1);
            }
        }
    }
}