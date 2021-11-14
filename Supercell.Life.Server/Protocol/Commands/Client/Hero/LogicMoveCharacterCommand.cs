namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicMoveCharacterCommand : LogicCommand
    {
        internal int DirectionX, DirectionY;

        internal byte Value;

        internal bool S, F;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMoveCharacterCommand"/> class.
        /// </summary>
        public LogicMoveCharacterCommand(Connection connection) : base(connection)
        {
            this.Type = Command.MoveCharacter;
        }

        internal override void Decode(ByteStream stream)
        {
            this.DirectionX = stream.ReadInt();
            this.DirectionY = stream.ReadInt();

            this.Value      = stream.ReadByte();

            this.S = this.Value == 1 || this.Value == 3;
            this.F = this.Value >= 2;

            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.DirectionX);
            encoder.WriteInt(this.DirectionY);

            encoder.WriteByte(this.Value);

            base.Encode(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var battle = gamemode.Battle;

            if (battle == null && gamemode.Avatar.Connection.State == State.Battle)
            {
                if (gamemode.Avatar.OngoingQuestData.Data.QuestType != "PvP")
                {
                    if (this.DirectionX + 256 >= 512)
                    {
                        Debugger.Error("Invalid Dx");
                        return;
                    }

                    if (this.DirectionY + 256 >= 512)
                    {
                        Debugger.Error("Invalid Dy");
                        return;
                    }

                    var dX = LogicMath.Abs(this.DirectionX) + 256;

                    if (LogicMath.Abs(this.DirectionY) + dX < 10)
                    {
                        return;
                    }

                    gamemode.Avatar.OngoingQuestData.SublevelMoveCount += 1;

                    var ongoingLevel = gamemode.Avatar.OngoingQuestData.Levels[gamemode.Avatar.OngoingQuestData.Level];
                    ongoingLevel.Battles[gamemode.Avatar.Quests[gamemode.Avatar.OngoingQuestData.GlobalID].Levels[0].CurrentBattle].CheckCollision(this.DirectionX, this.DirectionY);
                }
            }

            if (battle != null)
            {
                battle.ResetTurn(gamemode.Avatar);

                var opponent = battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier);

                LogicMoveCharacterCommand cmd = new LogicMoveCharacterCommand(opponent.Connection)
                {
                    DirectionX     = -this.DirectionX,
                    DirectionY     = -this.DirectionY,
                    Value          = this.Value,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.EnqueueCommand(this, cmd);
            }

            Debugger.Debug($"DirectionX : {this.DirectionX}, DirectionY : {this.DirectionY}, S : {this.S}, F : {this.F}");
        }

        internal override void LoadCommandFromJSON(LogicJSONObject json)
        {
            base.LoadCommandFromJSON(json);

            this.DirectionX = json.GetJsonNumber("dx").GetIntValue();
            this.DirectionY = json.GetJsonNumber("dy").GetIntValue();
            this.S          = json.GetJsonBoolean("s").IsTrue();
            this.F          = json.GetJsonBoolean("f").IsTrue();
        }

        internal override LogicJSONObject SaveCommandToJSON()
        {
            var json = base.SaveCommandToJSON();

            json.Put("dx", new LogicJSONNumber(this.DirectionX));
            json.Put("dy", new LogicJSONNumber(this.DirectionY));
            json.Put("s", new LogicJSONBoolean(this.S));
            json.Put("f", new LogicJSONBoolean(this.F));

            return json;
        }
    }
}