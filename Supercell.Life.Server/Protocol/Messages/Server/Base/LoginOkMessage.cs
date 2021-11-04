namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

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

            this.Stream.WriteString(null);
            this.Stream.WriteString(null);

            this.Stream.WriteInt(LogicVersion.Major);
            this.Stream.WriteInt(LogicVersion.Build);
            this.Stream.WriteInt(LogicVersion.Minor);

            this.Stream.WriteString(LogicVersion.ServerType);

            this.Stream.WriteInt(1); // Total Session
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.TimePlayed); // Played Time
            this.Stream.WriteInt(0); // Played Time in day

            this.Stream.WriteString(null); // 103121310241222
            this.Stream.WriteString(string.Empty);
            this.Stream.WriteString(string.Empty);

            this.Stream.WriteInt(00); // Remaining time before all functionality
        }
    }
}
