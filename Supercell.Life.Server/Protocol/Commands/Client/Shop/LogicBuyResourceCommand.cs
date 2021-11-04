namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicBuyResourceCommand : LogicCommand
    {
        internal Resource Resource;
        internal int Amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyResourceCommand"/> class.
        /// </summary>
        public LogicBuyResourceCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicBuyResourceCommand.
        }

        internal override void Decode()
        {
            this.Resource = (Resource)this.Stream.ReadInt();
            this.Amount   = this.Stream.ReadInt();

            this.Stream.ReadByte();

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            switch (this.Resource)
            {
                case Resource.Gold:
                {
                    int cost = this.DiamondCost;

                    if (gamemode.Avatar.Diamonds >= cost)
                    {
                        gamemode.Avatar.Diamonds -= cost;
                        gamemode.Avatar.AddGold(this.Amount);
                    }

                    break;
                }
                case Resource.Energy:
                {
                    int cost = this.Amount * 2;

                    if (gamemode.Avatar.Diamonds >= cost)
                    {
                        gamemode.Avatar.Diamonds -= cost;
                        gamemode.Avatar.EnergyTimer.Stop();
                        gamemode.Avatar.Energy = gamemode.Avatar.MaxEnergy;
                    }

                    break;
                }
            }
        }

        private int DiamondCost
        {
            get
            {
                switch (this.Amount)
                {
                    case 10:
                    {
                        return Globals.ResourceDiamondCost10;
                    }
                    case 100:
                    {
                        return Globals.ResourceDiamondCost100;
                    }
                    case 1000:
                    {
                        return Globals.ResourceDiamondCost1000;
                    }
                    case 10000:
                    {
                        return Globals.ResourceDiamondCost10000;
                    }
                    case 50000:
                    {
                        return Globals.ResourceDiamondCost50000;
                    }
                    case 100000:
                    {
                        return Globals.ResourceDiamondCost100000;
                    }
                    case 500000:
                    {
                        return Globals.ResourceDiamondCost500000;
                    }
                    case 1000000:
                    {
                        return Globals.ResourceDiamondCost1000000;
                    }
                    default:
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
