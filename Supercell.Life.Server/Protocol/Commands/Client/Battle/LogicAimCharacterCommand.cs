namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicAimCharacterCommand : LogicCommand
    {
        internal int DirectionX, DirectionY;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAimCharacterCommand"/> class.
        /// </summary>
        public LogicAimCharacterCommand(Connection connection) : base(connection)
        {
            this.Type = Command.AimCharacter;
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            this.DirectionX = stream.ReadInt();
            this.DirectionY = stream.ReadInt();
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            encoder.WriteInt(this.DirectionX);
            encoder.WriteInt(this.DirectionY);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Battle;

            if (battle != null)
            {
                LogicAimCharacterCommand cmd = new LogicAimCharacterCommand(battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier).Connection)
                {
                    DirectionX     = -this.DirectionX,
                    DirectionY     = -this.DirectionY,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };
                
                battle.EnqueueCommand(this, cmd);
            }
        }

        internal override void LoadCommandFromJSON(LogicJSONObject json)
        {
            base.LoadCommandFromJSON(json);

            this.DirectionX = json.GetJsonNumber("dx").GetIntValue();
            this.DirectionY = json.GetJsonNumber("dy").GetIntValue();
        }

        internal override LogicJSONObject SaveCommandToJSON()
        {
            var json = base.SaveCommandToJSON();

            json.Put("dx", new LogicJSONNumber(this.DirectionX));
            json.Put("dy", new LogicJSONNumber(this.DirectionY));

            return json;
        }
    }
}