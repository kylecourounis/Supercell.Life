namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicAimingArrowForReplayCommand : LogicCommand
    {
        internal int DirectionX, DirectionY;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAimingArrowForReplayCommand"/> class.
        /// </summary>
        public LogicAimingArrowForReplayCommand(Connection connection) : base(connection)
        {
            this.Type = Command.AimArrowForReplay;
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
            gamemode.Battle?.ReplayCommands.Add(this.SaveToJSON());
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