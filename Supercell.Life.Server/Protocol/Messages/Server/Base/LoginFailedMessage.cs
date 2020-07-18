namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System;
    
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LoginFailedMessage : PiranhaMessage
    {
        internal string ServerHost = "";
        internal string UpdateHost = "";

        internal string ErrorMessage;

        internal Reason Reason;

        /// <summary>
        /// Gets the time left before maintenance ends in seconds.
        /// </summary>
        internal int TimeLeft
        {
            get
            {
                return Settings.MaintenanceTime > DateTime.UtcNow ? (int) Settings.MaintenanceTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds : 0;
            }
        }

        /// <summary>
        /// Gets the patching host.
        /// </summary>
        internal string PatchingHost
        {
            get
            {
                return Fingerprint.Custom ? "https://raw.githubusercontent.com/kyledoescode/Patch/master/SmashLand/" : string.Empty;
            }
        }

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
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/> class.
        /// </summary>
        public LoginFailedMessage(Connection connection, Reason reason = Reason.Default, string errorMessage = "") : base(connection)
        {
            this.Type         = Message.LoginFailed;
            this.Version      = 9;
            this.Reason       = reason;
            this.ErrorMessage = errorMessage;
        }
        
        internal override void Encode()
        {
            this.Stream.WriteInt((int)this.Reason);

            this.Stream.WriteString(this.Reason == Reason.Patch ? Fingerprint.Json : null);
            this.Stream.WriteString(this.ServerHost);
            this.Stream.WriteString(this.PatchingHost);
            this.Stream.WriteString(this.UpdateHost);
            this.Stream.WriteString(this.ErrorMessage);

            this.Stream.WriteByte(0);
            this.Stream.WriteInt(this.TimeLeft);
            this.Stream.WriteString(null);
            this.Stream.WriteString(null);
            this.Stream.WriteInt(-43); 
            this.Stream.WriteInt(0);
            this.Stream.WriteString(null);
            this.Stream.WriteString(null);
            this.Stream.WriteByte(1);
        }
    }
}