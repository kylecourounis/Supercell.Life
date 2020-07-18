namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands;

    internal class EndClientTurnMessage : PiranhaMessage
    {
        internal int SubTick;
        internal int Checksum;
        internal int CommandCount;

        internal LogicArrayList<LogicCommand> Commands;

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
            this.SubTick      = this.Stream.ReadInt();
            this.Checksum     = this.Stream.ReadInt();
            this.CommandCount = this.Stream.ReadInt();
            
            if (this.CommandCount <= 512)
            {
                if (this.CommandCount > 0)
                {
                    this.Commands = new LogicArrayList<LogicCommand>(this.CommandCount);

                    for (int i = 0; i < this.CommandCount; i++)
                    {
                        int commandId = this.Stream.ReadInt();

                        if (commandId > 15)
                        {
                            LogicCommand command = LogicCommandManager.CreateCommand(commandId, this.Connection, this.Stream);

                            if (command != null)
                            {
                                if (LogicCommandManager.IsCommandAllowedInCurrentState(command))
                                {
                                    Debugger.Info($"Command {command.GetType().Name.Pad(34)} received from {this.Connection.EndPoint}.");

                                    command.Subtick = this.SubTick;
                                    command.Decode();

                                    this.Commands.Add(command);
                                }
                            }
                            else this.ShowBuffer();
                        }
                    }
                }
            }
        }

        internal override void Handle()
        {
            if (this.Commands != null)
            {
                while (this.Commands.Size > 0)
                {
                    LogicCommand command = this.Commands[0];

                    if (command.ExecuteSubTick != -1)
                    {
                        if (command.ExecuteSubTick <= this.SubTick)
                        {
                            if (this.Connection.Avatar.Time.ClientSubTick <= command.ExecuteSubTick)
                            {
                                this.Connection.Avatar.Time.ClientSubTick = command.ExecuteSubTick;
                                this.Connection.Avatar.Tick();
                                
                                command.Execute();
                            }
                        }
                        else Debugger.Error($"Execute command failed! Command should already executed. (type={command.Type}, server_tick)");
                    }
                    else Debugger.Error("Execute command failed! subtick is not valid.");

                    this.Commands.RemoveAt(0);
                }
            }

            this.Connection.Avatar.Time.ClientSubTick = this.SubTick;
            this.Connection.Avatar.Tick();
            
            Debugger.Debug($"Client Time :  MS={this.Connection.Avatar.Time.TotalMS}   SECS={this.Connection.Avatar.Time.TotalSecs}.");
            Debugger.Debug($"Checksum    :  CLIENT={this.Checksum - this.Connection.Avatar.Time.ClientSubTick}   SERVER={this.Connection.Avatar.Checksum}.");
        }
    }
}