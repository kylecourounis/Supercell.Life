namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Titan.Helpers;

    internal class LoginOkMessage : PiranhaMessage
    {
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
        /// Initializes a new instance of the <see cref="LoginOkMessage"/> class.
        /// </summary>
        public LoginOkMessage(Connection connection) : base(connection)
        {
            this.Type             = Message.LoginOk;
            this.Connection.State = State.LoggedIn;
        }
        
        internal override void Encode()
        {
            this.Stream.WriteLogicLong(this.Connection.GameMode.Avatar.Identifier);
            this.Stream.WriteLogicLong(this.Connection.GameMode.Avatar.Identifier);

            this.Stream.WriteString(this.Connection.GameMode.Avatar.Token);

            this.Stream.WriteString(this.Connection.GameMode.Avatar.Facebook.Identifier);
            this.Stream.WriteString(null); // GameCenter ID

            this.Stream.WriteInt(LogicVersion.Major);
            this.Stream.WriteInt(LogicVersion.Build);
            this.Stream.WriteInt(LogicVersion.Minor);

            this.Stream.WriteString(LogicVersion.ServerEnvironment);

            this.Stream.WriteInt(this.Connection.GameMode.Avatar.TotalSessions++); // Total Session
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.TimePlayed.Seconds); // Played Time
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.TimePlayed.Days); // Days Since Creation

            this.Stream.WriteString(null); // Facebook Application ID
            this.Stream.WriteString(Timestamp.Milliseconds.ToString()); // Server Time
            this.Stream.WriteString(string.Empty);

            this.Stream.WriteInt(0); // Remaining time before all functionality
        }
    }
}
