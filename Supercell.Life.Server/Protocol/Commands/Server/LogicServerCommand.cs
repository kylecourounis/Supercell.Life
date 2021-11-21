namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicServerCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicServerCommand"/> class.
        /// </summary>
        internal LogicServerCommand(Connection connection) : base(connection)
        {
            // LogicServerCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal new virtual void Decode(ByteStream stream)
        {
            this.Type           = (Command)stream.ReadInt();
            this.ExecuteSubTick = stream.ReadInt();
            this.ExecutorID     = stream.ReadLogicLong();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal new virtual void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt((int)this.Type);
            encoder.WriteInt(this.ExecuteSubTick);
            encoder.WriteLogicLong(this.Connection.GameMode.Avatar.Identifier);
        }
    }
}
