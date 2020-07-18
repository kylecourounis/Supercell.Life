namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System.Collections.Generic;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class JoinableAllianceListMessage : PiranhaMessage
    {
        internal List<Alliance> JoinableAlliances;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Alliance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListMessage"/> class.
        /// </summary>
        internal JoinableAllianceListMessage(Connection connection) : base(connection)
        {
            this.Type              = Message.JoinableAlliances;
            this.JoinableAlliances = Alliances.GetRandomAlliances();
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.JoinableAlliances.Count);

            this.JoinableAlliances.ForEach(alliance =>
            {
                alliance.EncodeHeader(this.Stream);
            });
        }
    }
}
