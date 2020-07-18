namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;

    internal class LogicUnknownCommand : LogicCommand
    {
        internal int Unk_1;
        internal int Unk_2;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUnknownCommand"/> class.
        /// </summary>
        public LogicUnknownCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicUnknownCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();

            this.Unk_1 = this.Stream.ReadInt();
            this.Unk_2 = this.Stream.ReadInt();

            Debugger.Debug(this.Unk_1 + ", " + this.Unk_2);
        }

        internal override void Execute()
        {
            // Debugger.Debug($"Subtick={this.ExecuteSubTick}, ExecutorID={this.ExecutorID}; Unk_1={this.Unk_1}, Unk_2={this.Unk_2}");
        }
    }
}