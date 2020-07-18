namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicAttachItemCommand : LogicCommand
    {
        internal LogicHeroData Hero;
        internal LogicItemsData Item;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAttachItemCommand"/> class.
        /// </summary>
        public LogicAttachItemCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicAttachItemCommand.
        }

        internal override void Decode()
        {
            this.Item = this.Stream.ReadDataReference<LogicItemsData>();
            this.Hero = this.Stream.ReadDataReference<LogicHeroData>();

            this.ReadHeader();
        }

        internal override void Execute()
        {
            if (!this.Connection.Avatar.ItemAttachedTo.ContainsKey(this.Item.GlobalID))
            {
                this.Connection.Avatar.ItemAttachedTo.AddItem(this.Item.GlobalID, this.Hero.GlobalID);
            }
            else
            {
                this.Connection.Avatar.ItemAttachedTo.Remove(this.Item.GlobalID);
                this.Connection.Avatar.ItemUnavailableTimer.Start(this.Item.GlobalID);
            }

            this.Connection.Avatar.Save();
        }
    }
}