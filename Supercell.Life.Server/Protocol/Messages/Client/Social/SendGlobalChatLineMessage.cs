namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using System;
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands;
    using Supercell.Life.Server.Protocol.Commands.Debugs;

    internal class SendGlobalChatLineMessage : PiranhaMessage
    {
        private string Message = string.Empty;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGlobalChatLineMessage"/> class.
        /// </summary>
        public SendGlobalChatLineMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SendGlobalChatLineMessage.
        }
        
        internal override void Decode()
        {
            this.Message = this.Stream.ReadString();
        }

        internal override void Handle()
        {
            if (this.Message.StartsWith(LogicChatCommands.Delimiter))
            {
                string[] commands = this.Message.Remove(0, 1).Split(' ');

                if (commands.Length > 0)
                {
                    string command     = commands[0];
                    string[] arguments = commands.Skip(1).ToArray();

                    LogicChatCommand debug = LogicChatCommands.CreateChatCommand(command, this.Connection, arguments);

                    if (debug != null)
                    {
                        Debugger.Info($"Chat Command {command} has been handled.");

                        try
                        {
                            debug.Process();
                        }
                        catch
                        {
                            Debugger.Error($"Failed to handle the chat command : {command}!");
                        }
                    }
                }
            }
            else
            {
                this.Connection.SendChatMessage(this.Message, false);
            }
        }
    }
}
