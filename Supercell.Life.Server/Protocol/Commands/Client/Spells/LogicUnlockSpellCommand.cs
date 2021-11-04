namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicUnlockSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUnlockSpellCommand"/> class.
        /// </summary>
        public LogicUnlockSpellCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicUnlockSpellCommand.
        }

        internal override void Decode()
        {
            this.Spell = this.Stream.ReadDataReference<LogicSpellData>();

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (!gamemode.Avatar.Spells.ContainsKey(this.Spell.GlobalID))
            {
                if (gamemode.Avatar.Gold >= this.Spell.UnlockCost)
                {
                    gamemode.Avatar.Gold -= this.Spell.UnlockCost;

                    gamemode.Avatar.Spells.AddItem(this.Spell.GlobalID, 0);
                    gamemode.Avatar.SpellsReady.AddItem(this.Spell.GlobalID, 1);
                }
            }
        }
    }
}