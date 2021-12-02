namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicSwapCharacterCommand : LogicCommand
    {
        internal int Identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSwapCharacterCommand"/> class.
        /// </summary>
        public LogicSwapCharacterCommand(Connection connection) : base(connection)
        {
            this.Type = Command.SwapCharacter;
        }
        
        internal override void Decode(ByteStream stream)
        {
            this.Identifier = stream.ReadInt();

            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.Identifier);
            
            base.Encode(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Battle;

            if (battle != null)
            {
                var enemy = battle.GetEnemy(gamemode.Avatar);

                // int character = this.Connection.GameMode.Avatar.Team[GlobalID.GetID(this.Identifier)].ToObject<int>();

                battle.Turn.CharacterIndex = GlobalID.GetID(this.Identifier);

                var cmd = new LogicSwapCharacterCommand(enemy.Connection)
                {
                    // Identifier     = this.Identifier,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.EnqueueCommand(this, cmd);
            }
            else
            {
                gamemode.Avatar.OngoingQuestData.CharacterIndex = GlobalID.GetID(this.Identifier);
            }
        }

        internal override void LoadCommandFromJSON(LogicJSONObject json)
        {
            base.LoadCommandFromJSON(json);

            this.Identifier = json.GetJsonNumber("id").GetIntValue();
        }

        internal override LogicJSONObject SaveCommandToJSON()
        {
            var json = base.SaveCommandToJSON();

            json.Put("id", new LogicJSONNumber(this.Identifier));

            return json;
        }
    }
}
