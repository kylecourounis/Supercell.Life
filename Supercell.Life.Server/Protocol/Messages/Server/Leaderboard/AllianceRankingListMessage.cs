namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AllianceRankingListMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Ranking;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRankingListMessage"/> class.
        /// </summary>
        internal AllianceRankingListMessage(Connection connection) : base(connection)
        {
            this.Type = Message.AllianceRankingList;
        }
        
        internal override void Encode()
        {
            var allianceList = Alliances.OrderByDescending();

            this.Stream.WriteInt(allianceList.Count);

            foreach (Alliance alliance in allianceList)
            {
                alliance.EncodeRankingEntry(this.Stream);
            }
        }
    }
}
