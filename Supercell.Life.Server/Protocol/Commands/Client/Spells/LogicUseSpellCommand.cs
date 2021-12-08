namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicUseSpellCommand : LogicCommand
    {
        internal LogicSpellData Spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUseSpellCommand"/> class.
        /// </summary>
        public LogicUseSpellCommand(Connection connection) : base(connection)
        {
            this.Type = Command.UseSpell;
        }

        internal override void Decode(ByteStream stream)
        {
            this.Spell = stream.ReadDataReference<LogicSpellData>();

            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteDataReference(this.Spell);

            base.Encode(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.SpellsReady.ContainsKey(this.Spell.GlobalID))
            {
                gamemode.Avatar.Spells.AddPreviousSpell(this.Spell.GlobalID, 1);
                gamemode.Avatar.SpellsReady.Remove(this.Spell.GlobalID, 1);

                if (gamemode.Avatar.SpellsReady.GetCount(this.Spell.GlobalID) == 0)
                {
                    gamemode.Avatar.SpellsReady.Remove(this.Spell.GlobalID);
                }
            }

            var battle = gamemode.Battle;

            if (battle != null)
            {
                var enemy = battle.GetEnemy(gamemode.Avatar);

                LogicUseSpellCommand cmd = new LogicUseSpellCommand(enemy.Connection)
                {
                    Spell          = this.Spell,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.EnqueueCommand(this, cmd);
            }
        }

        internal override void LoadCommandFromJSON(LogicJSONObject json)
        {
            base.LoadCommandFromJSON(json);

            this.Spell = (LogicSpellData)CSV.Tables.GetWithGlobalID(json.GetJsonNumber("id").GetIntValue());
        }

        internal override LogicJSONObject SaveCommandToJSON()
        {
            var json = base.SaveCommandToJSON();

            json.Put("id", new LogicJSONNumber(this.Spell.GlobalID));

            return json;
        }
    }
}