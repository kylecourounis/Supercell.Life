namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System.Text;
    
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class ServerErrorMessage : PiranhaMessage
    {
        private readonly string Text;
        private readonly StringBuilder Reason;

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
        /// Initializes a new instance of the <see cref="ServerErrorMessage"/> class.
        /// </summary>
        public ServerErrorMessage(Connection connection, string text = "", bool initial = false) : base(connection)
        {
            this.Type   = Message.ServerError;
            this.Text   = text;
            this.Reason = new StringBuilder();

            if (!initial)
            {
                this.Reason.AppendLine("Your game threw an exception on the server!\nPlease contact one of the Brokencell Developers with the following information :");

                if (this.Connection.Avatar != null)
                {
                    this.Reason.AppendLine($"Your Player Name    : {this.Connection.Avatar.Name}.");
                    this.Reason.AppendLine($"Your Player ID      : {this.Connection.Avatar.HighID}-{this.Connection.Avatar.LowID}.");
                }

                this.Reason.AppendLine();
                this.Reason.AppendLine("Trace : ");
            }

            this.Reason.AppendLine(this.Text);
        }
        
        internal override void Encode()
        {
            this.Stream.WriteString(this.Reason.ToString());
        }
    }
}