namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicUnlockSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUnlockSpellCommand"/> class.
        /// </summary>
        public LogicUnlockSpellCommand(Connection connection) : base(connection)
        {
            // LogicUnlockSpellCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Spell = stream.ReadDataReference<LogicSpellData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (!gamemode.Avatar.Spells.ContainsKey(this.Spell.GlobalID))
            {
                if (gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -this.Spell.UnlockCost))
                {
                    gamemode.Avatar.Spells.AddItem(this.Spell.GlobalID, 0);
                    gamemode.Avatar.SpellsReady.AddItem(this.Spell.GlobalID, 1);
                }
            }
        }
    }
}