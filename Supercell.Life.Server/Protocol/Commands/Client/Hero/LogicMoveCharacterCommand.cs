namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Network;

    internal class LogicMoveCharacterCommand : LogicCommand
    {
        internal int DirectionX;
        internal int DirectionY;

        internal bool S, F;

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

            byte value = this.Stream.ReadByte();

            this.S = value == 1 || value == 3;
            this.F = value >= 2;

            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (this.Connection.Avatar.CurrentQuest.Data.QuestType != "PvP")
            {
                this.Connection.Avatar.CurrentQuest.Moves += 1;
            }

            Debugger.Debug($"DirectionX : {this.DirectionX}, DirectionY : {this.DirectionY}, S : {this.S}, F : {this.F}");
        }
    }
}