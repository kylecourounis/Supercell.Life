namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
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

        internal override void Execute()
        {
            foreach (LogicSpellData spell in this.Spells)
            {
                var count = this.Connection.Avatar.SpellsReady.Values.Sum(item => item.Count);
                Debugger.Debug(count);
                
                if (count < this.Connection.Avatar.Variables.Get(LogicVariables.SpellSlotsUnlocked.GlobalID).Count + 2)
                {
                    if (this.Connection.Avatar.Gold >= spell.CreateCost)
                    {
                        this.Connection.Avatar.Gold -= spell.CreateCost;
                        this.Connection.Avatar.SpellTimer.SpellIDs.Add(spell.GlobalID);
                        this.Connection.Avatar.SpellTimer.Start();
                    }
                }
                else
                {
                    Debugger.Error($"Spell slots maxed out for player {this.Connection.Avatar}");
                }
            }

            this.Connection.Avatar.Save();
        }
    }
}