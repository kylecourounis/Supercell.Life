namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMoveCharacterCommand"/> class.
        /// </summary>
        public LogicMoveCharacterCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicMoveCharacterCommand.
        }

        internal override void Decode()
        {
            this.DirectionX = this.Stream.ReadInt();
            this.DirectionY = this.Stream.ReadInt();

            this.Value      = this.Stream.ReadByte();

            this.S = this.Value == 1 || this.Value == 3;
            this.F = this.Value >= 2;

            this.ReadHeader();
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.DirectionX);
            this.Stream.WriteInt(this.DirectionY);

            this.Stream.WriteByte(this.Value);

            this.WriteHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.OngoingQuestData != null)
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

            var battle = gamemode.Avatar.Battle;

            if (battle != null)
            {
                battle.Turn++;

                var opponent = battle.Avatars.Find(avatar => avatar.Identifier != gamemode.Avatar.Identifier);

                LogicMoveCharacterCommand cmd = new LogicMoveCharacterCommand(opponent.Connection)
                {
                    DirectionX     = this.DirectionX,
                    DirectionY     = this.DirectionY,
                    Value          = this.Value,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.GetOwnQueue(gamemode.Avatar).Enqueue(this);
                battle.GetEnemyQueue(gamemode.Avatar).Enqueue(cmd);
            }

            Debugger.Debug($"DirectionX : {this.DirectionX}, DirectionY : {this.DirectionY}, S : {this.S}, F : {this.F}");
        }
    }
}