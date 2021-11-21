namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Game.Objects;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    
    internal class GlobalChatLineMessage : PiranhaMessage
    {
        internal GlobalChatLine Chat;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.GlobalChat;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalChatLineMessage"/> class.
        /// </summary>
        internal GlobalChatLineMessage(Connection connection) : base(connection)
        {
            this.Type = Message.GlobalChatLine;
        }

        internal override void Encode()
        {
            this.Stream.WriteString(this.Chat.Message);
            this.Stream.WriteString(this.Chat.System ? "System" : this.Chat.WhoSent ? $"[{this.Chat.Sender.Rank}] You" : this.Chat.Regex ? $"[{this.Chat.Sender.Rank}] {this.Chat.Sender.Name}" : this.Chat.Sender.Name);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(this.Chat.Sender.League);

            this.Stream.WriteLogicLong(this.Chat.Sender.Identifier);
            this.Stream.WriteLogicLong(this.Chat.Sender.Identifier);
        }
    }
}
