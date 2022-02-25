namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands;

    internal class EndClientTurnMessage : PiranhaMessage
    {
        internal int Subtick;
        internal int Checksum;
        internal int CommandCount;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Home;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndClientTurnMessage"/> class.
        /// </summary>
        public EndClientTurnMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // EndClientTurnMessage.
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
                        this.Connection.GameMode.CommandManager.DecodeCommand(this.Stream, this.Subtick);
                    }
                }
            }
        }

        internal override void Handle()
        {
            LogicCommandManager commandManager = this.Connection.GameMode.CommandManager;

            if (this.CommandCount > 0)
            {
                if (commandManager.Commands != null)
                {
                    do
                    {
                        commandManager.ExecuteCommand(commandManager.Commands[0]);
                    } while (commandManager.Commands.Size > 0);

                    this.Connection.GameMode.Avatar.Save();
                }
            }

            this.Connection.GameMode.Avatar.Time.ClientSubTick = this.Subtick;
            this.Connection.GameMode.Tick();

            Debugger.Debug($"Client Time :  MS={this.Connection.GameMode.Avatar.Time.TotalMS}   SECS={this.Connection.GameMode.Avatar.Time.TotalSecs}.");
            Debugger.Debug($"Checksum    :  CLIENT={this.Checksum - this.Connection.GameMode.Avatar.Time.ClientSubTick}   SERVER={this.Connection.GameMode.Avatar.Checksum}.");
        }
    }
}