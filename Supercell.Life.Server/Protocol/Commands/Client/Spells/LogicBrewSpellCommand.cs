namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicBrewSpellCommand : LogicCommand
    {
        internal LogicArrayList<LogicSpellData> Spells;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBrewSpellCommand"/> class.
        /// </summary>
        public LogicBrewSpellCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Spells = new LogicArrayList<LogicSpellData>();
        }

        internal override void Decode()
        {
            var count = (int)this.Stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                this.Spells.Add(this.Stream.ReadDataReference<LogicSpellData>());
            }

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            foreach (LogicSpellData spell in this.Spells)
            {
                var count = gamemode.Avatar.SpellsReady.Values.Sum(item => item.Count);
                
                if (count < gamemode.Avatar.Variables.Get(LogicVariables.SpellSlotsUnlocked.GlobalID).Count + 2)
                {
                    if (gamemode.Avatar.Gold >= spell.CreateCost)
                    {
                        gamemode.Avatar.Gold -= spell.CreateCost;
                        gamemode.Avatar.SpellTimer.SpellIDs.Add(spell.GlobalID);
                        gamemode.Avatar.SpellTimer.Start();
                    }
                }
                else
                {
                    Debugger.Error($"Spell slots maxed out for player {gamemode.Avatar}");
                }
            }
        }
    }
}