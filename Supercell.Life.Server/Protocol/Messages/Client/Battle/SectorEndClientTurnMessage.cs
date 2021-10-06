namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands;

    internal class SectorEndClientTurnMessage : PiranhaMessage
    {
        internal int SubTick;
        internal int Checksum;
        internal int CommandCount;

        internal LogicArrayList<LogicCommand> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorEndClientTurnMessage"/> class.
        /// </summary>
        public SectorEndClientTurnMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SectorEndClientTurnMessage.
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
                        int commandID = this.Stream.ReadInt();

                        if (commandID >= 600)
                        {
                            LogicCommand command = LogicCommandManager.CreateCommand(commandID, this.Connection, this.Stream);

                            if (command != null)
                            {
                                if (LogicCommandManager.IsCommandAllowedInCurrentState(command))
                                {
                                    Debugger.Info($"Battle Command {command.GetType().Name.Pad(34)} received from {this.Connection.EndPoint}.");

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
                                this.Connection.Avatar.GameMode.Tick();

                                command.Execute();
                            }
                        }
                        else Debugger.Error($"Execute command failed! Command should already executed. (type={command.Type}, server_tick)");
                    }
                    else Debugger.Error("Execute command failed! subtick is not valid.");

                    this.Commands.RemoveAt(0);
                }
            }
        }
    }
}