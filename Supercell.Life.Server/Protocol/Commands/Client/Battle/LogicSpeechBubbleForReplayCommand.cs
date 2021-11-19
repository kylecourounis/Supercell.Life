namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicSpeechBubbleForReplayCommand : LogicCommand
    {
        internal int PlayerIndex;

        internal int TauntIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpeechBubbleForReplayCommand"/> class.
        /// </summary>
        public LogicSpeechBubbleForReplayCommand(Connection connection) : base(connection)
        {
            this.Type = Command.SpeechBubbleReplay;
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            this.PlayerIndex = stream.ReadInt();
            this.TauntIndex  = stream.ReadInt();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Battle;

            this.ShowValues();

            if (battle != null)
            {
                var enemy = battle.GetEnemy(gamemode.Avatar);

                LogicTauntData taunt = (LogicTauntData)battle.GameModes[this.PlayerIndex].Avatar.Extras.Values.ToList().FindAll(slot => slot.Id >= 24000000)[this.TauntIndex].Data;

            }
        }

        internal override void LoadCommandFromJSON(LogicJSONObject json)
        {
            base.LoadCommandFromJSON(json);

            this.PlayerIndex = json.GetJsonNumber("playerIndex").GetIntValue();
            this.TauntIndex  = json.GetJsonNumber("tauntIndex").GetIntValue();
        }

        internal override LogicJSONObject SaveCommandToJSON()
        {
            var json = base.SaveCommandToJSON();

            json.Put("playerIndex", new LogicJSONNumber(this.PlayerIndex));
            json.Put("tauntIndex", new LogicJSONNumber(this.TauntIndex));

            return json;
        }
    }
}