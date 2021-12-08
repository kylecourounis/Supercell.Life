namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicBrewSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBrewSpellCommand"/> class.
        /// </summary>
        public LogicBrewSpellCommand(Connection connection) : base(connection)
        {
            // LogicBrewSpellCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            if (stream.ReadBoolean())
            {
                this.Spell = stream.ReadDataReference<LogicSpellData>();
            }

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            this.ShowValues();

            int count = gamemode.Avatar.SpellsReady.Values.Sum(item => item.Count);

            if (count < gamemode.Avatar.Variables.Get(LogicVariables.SpellSlotsUnlocked.GlobalID).Count + Globals.SpellSlotsAtStart)
            {
                if (gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -this.Spell.CreateCost))
                {
                    gamemode.Avatar.SpellTimer.AddSpell(this.Spell);
                }
            }
            else
            {
                Debugger.Error($"Spell slots maxed out for player {gamemode.Avatar}");
            }
        }
    }
}