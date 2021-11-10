namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicAttachItemCommand : LogicCommand
    {
        internal LogicHeroData Hero;
        internal LogicItemsData Item;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAttachItemCommand"/> class.
        /// </summary>
        public LogicAttachItemCommand(Connection connection) : base(connection)
        {
            // LogicAttachItemCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Item = stream.ReadDataReference<LogicItemsData>();
            this.Hero = stream.ReadDataReference<LogicHeroData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (!gamemode.Avatar.ItemAttachedTo.ContainsKey(this.Item.GlobalID))
            {
                gamemode.Avatar.ItemAttachedTo.AddItem(this.Item.GlobalID, this.Hero.GlobalID);
            }
            else
            {
                gamemode.Avatar.ItemAttachedTo.Remove(this.Item.GlobalID);
                gamemode.Avatar.ItemUnavailableTimer.Start(this.Item.GlobalID);
            }
        }
    }
}