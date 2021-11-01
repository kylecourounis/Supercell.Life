namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands;

    internal class SectorEndClientTurnMessage : PiranhaMessage
    {
        internal int Subtick;
        internal int Checksum;
        internal int CommandCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorEndClientTurnMessage"/> class.
        /// </summary>
        public SectorEndClientTurnMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SectorEndClientTurnMessage.
        }

        internal override void Decode()
        {
            this.Subtick      = this.Stream.ReadInt();
            this.Checksum     = this.Stream.ReadInt();
            this.CommandCount = this.Stream.ReadInt();

            if (this.CommandCount <= 512)
            {
                if (this.CommandCount > 0)
                {
                    for (int i = 0; i < this.CommandCount; i++)
                    {
                        this.Connection.Messaging.CommandManager.DecodeBattleCommand(this.Stream, this.Subtick);
                    }
                }
            }
        }

        internal override void Handle()
        {
            LogicCommandManager commandManager = this.Connection.Messaging.CommandManager;

            if (commandManager.SectorCommands != null)
            {
                while (commandManager.SectorCommands.Size > 0)
                {
                    commandManager.ExecuteCommand(commandManager.SectorCommands[0]);
                    commandManager.SectorCommands.RemoveAt(0);
                }
            }
        }
    }
}