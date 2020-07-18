namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Slots.Items;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    
    internal class GlobalChatLineMessage : PiranhaMessage
    {
        internal GlobalChatItem Chat;

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
            this.Stream.WriteString(this.Chat.System ? "System" : this.Chat.WhoSent ? $"[{this.Connection.Avatar.Rank}] You" : this.Chat.Regex ? $"[{this.Connection.Avatar.Rank}] {this.Connection.Avatar.Name}" : this.Connection.Avatar.Name);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(this.Connection.Avatar.League);

            this.Stream.WriteLong(this.Connection.Avatar.Identifier);
            this.Stream.WriteLong(this.Connection.Avatar.Identifier);
        }
    }
}
