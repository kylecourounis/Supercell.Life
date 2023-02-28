namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using System;
    using System.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LoginMessage : PiranhaMessage
    {
        private LogicLong AvatarID;
        
        private string Token;

        private int Major;
        private int Minor;
        private int Build;
        private string MasterHash;
        
        private int Seed;

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
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        public LoginMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Connection.State = State.Login;
        }
        
        internal override void Decode()
        {
            this.AvatarID               = this.Stream.ReadLogicLong();
            this.Token                  = this.Stream.ReadString();
            
            this.Major                  = this.Stream.ReadInt();
            this.Minor                  = this.Stream.ReadInt();
            this.Build                  = this.Stream.ReadInt();
            this.MasterHash             = this.Stream.ReadString();

            this.Stream.ReadString();
            this.Connection.UDID        = this.Stream.ReadString();
            this.Connection.MACAddress  = this.Stream.ReadString();
            this.Connection.DeviceModel = this.Stream.ReadString();
            
            this.Stream.ReadBytes(4);

            if (!this.Stream.IsAtEnd)
            {
                this.Connection.PreferredLanguage = this.Stream.ReadString();

                if (!this.Stream.IsAtEnd)
                {
                    this.Connection.ADID          = this.Stream.ReadString();

                    if (!this.Stream.IsAtEnd)
                    {
                        this.Connection.OSVersion = this.Stream.ReadString();
                        this.Connection.Android   = this.Stream.ReadBoolean(); // This will always be false

                        this.Stream.ReadInt(); // 0

                        this.Stream.ReadByte(); // 0
                        this.Stream.ReadByte(); // 0
                        this.Stream.ReadByte(); // 0
                        this.Stream.ReadByte(); // 0

                        this.Stream.ReadString();

                        this.Connection.Advertising = this.Stream.ReadBoolean();

                        this.Stream.ReadString(); 

                        this.Seed                   = this.Stream.ReadInt();
                    }
                }
            }
        }

        internal override void Handle()
        {
            // if (this.Trusted)
            {
                if (this.AvatarID.Low == 0)
                {
                    this.Connection.GameMode.Avatar = Avatars.Create(this.Connection);

                    Debugger.Warning($"Player not found! Creating {this.Connection.GameMode.Avatar.Identifier}...");

                    if (this.Connection.GameMode.Avatar != null)
                    {
                        this.Login();
                    }
                    else
                    {
                        new LoginFailedMessage(this.Connection, Reason.Pause).Send();
                    }
                }
                else if (this.AvatarID.Low > 0)
                {
                    this.Connection.GameMode.Avatar = Avatars.Get(this.Connection, this.AvatarID);

                    if (this.Connection.GameMode.Avatar != null)
                    {
                        Debugger.Info($"{this.AvatarID} found! Logging in...");
                    }
                    else
                    {
                        Debugger.Info($"{this.AvatarID} not found! Creating...");

                        this.Connection.GameMode.Avatar = Avatars.Create(this.Connection);
                    }

                    this.Login();
                }
                else
                {
                    Debugger.Error("Player tried to login with a player id inferior to 0.");
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the connected <see cref="Connection"/> should be trusted.
        /// </summary>
        internal bool Trusted
        {
            get
            {
                if (LogicVersion.IsIntegration)
                {
                    string ipAddress = this.Connection.EndPoint.ToString();

                    if (!(Settings.AuthorizedIP.Contains(ipAddress.Split(':')[0]) || ipAddress.StartsWith("192.168.")))
                    {
                        new LoginFailedMessage(this.Connection, Reason.Maintenance).Send();
                        return false;
                    }
                }

                if (this.Major == LogicVersion.Major && this.Build == LogicVersion.Build)
                {
                    if (Settings.MaintenanceTime < DateTime.UtcNow)
                    {
                        if (string.Equals(this.MasterHash, Fingerprint.Sha))
                        {
                            return true;
                        }

                        new LoginFailedMessage(this.Connection, Reason.Patch).Send();
                    }
                    else
                    {
                        new LoginFailedMessage(this.Connection, Reason.Maintenance).Send();
                    }
                }
                else
                {
                    new LoginFailedMessage(this.Connection, Reason.Update).Send();
                }

                return false;
            }
        }
        
        /// <summary>
        /// Logs the player in.
        /// </summary>
        private void Login()
        {
            this.Connection.GameMode.Avatar.Connection = this.Connection;

            this.Connection.GameMode.Random.SetIteratedRandomSeed(this.Seed);

            new LoginOkMessage(this.Connection).Send();
            new OwnAvatarDataMessage(this.Connection).Send();

            if (this.Connection.GameMode.Avatar.IsInAlliance)
            {
                new AllianceStreamMessage(this.Connection).Send();
            }
        }
    }
}
