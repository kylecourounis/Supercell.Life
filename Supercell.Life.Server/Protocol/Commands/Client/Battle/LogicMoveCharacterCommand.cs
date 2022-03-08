namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;
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
            LogicVector2 vector = new LogicVector2(256 - LogicMath.Abs(this.DirectionX), LogicMath.Abs(this.DirectionY));

            Debugger.Debug($"DirectionX : {this.DirectionX}, DirectionY : {this.DirectionY}, Value : {this.Value} (S : {this.S}, F : {this.F})");
            Debugger.Debug($"Coords : {vector}, Angle : {vector.Angle}");

            var battle = gamemode.Battle;

            if (battle == null)
            {
                LogicQuest ongoingQuest = gamemode.Avatar.OngoingQuestData;

                if (ongoingQuest.Data.QuestType.Equals("Basic"))
                {
                    if (this.DirectionX + 256 > 512)
                    {
                        Debugger.Error("Invalid Dx");
                        return;
                    }

                    if (this.DirectionY + 256 > 512)
                    {
                        Debugger.Error("Invalid Dy");
                        return;
                    }

                    var dX = LogicMath.Abs(this.DirectionX) + 256;

                    if (LogicMath.Abs(this.DirectionY) + dX < 10)
                    {
                        return;
                    }

                    ongoingQuest.SublevelMoveCount++;
                    
                    LogicLevel ongoingLevel = ongoingQuest.Levels[gamemode.Avatar.OngoingQuestData.ReplayLevel];

                    if (!gamemode.Avatar.OngoingQuestData.IsReplaying)
                    {
                        ongoingLevel = ongoingQuest.Levels[ongoingQuest.Level];
                    }

                    ongoingLevel?.Battles[ongoingLevel.CurrentBattle].CheckCollision(vector);
                }
            }
            else
            {
                var opponent = battle.GetEnemy(gamemode.Avatar);

                battle.Turn.SetCharacterPosition(vector);
                
                LogicMoveCharacterCommand cmd = new LogicMoveCharacterCommand(opponent.Connection)
                {
                    DirectionX     = this.DirectionX,
                    DirectionY     = this.DirectionY,
                    Value          = this.Value,
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