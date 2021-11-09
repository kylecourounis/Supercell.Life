namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Collections.Items;
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
            this.Stream.WriteString(this.Chat.System ? "System" : this.Chat.WhoSent ? $"[{this.Connection.GameMode.Avatar.Rank}] You" : this.Chat.Regex ? $"[{this.Connection.GameMode.Avatar.Rank}] {this.Connection.GameMode.Avatar.Name}" : this.Connection.GameMode.Avatar.Name);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.League);

            this.Stream.WriteLong(this.Connection.GameMode.Avatar.Identifier);
            this.Stream.WriteLong(this.Connection.GameMode.Avatar.Identifier);
        }
    }
}
