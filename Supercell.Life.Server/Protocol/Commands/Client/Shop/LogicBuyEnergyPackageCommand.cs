namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicBuyEnergyPackageCommand : LogicCommand
    {
        internal LogicEnergyPackageData EnergyPackage;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyEnergyPackageCommand"/> class.
        /// </summary>
        public LogicBuyEnergyPackageCommand(Connection connection) : base(connection)
        {
            // LogicBuyEnergyPackageCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.EnergyPackage = stream.ReadDataReference<LogicEnergyPackageData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (this.EnergyPackage != null)
            {
                int alreadyBought = gamemode.Avatar.EnergyPackages.GetCount(this.EnergyPackage.GlobalID);

                if (this.EnergyPackage.Diamonds.Count > alreadyBought)
                {
                    int cost = this.EnergyPackage.Diamonds[alreadyBought];

                    if (cost > 0)
                    {
                        if (gamemode.Avatar.Diamonds < cost)
                        {
                            Debugger.Error($"Unable to buy a energy package. {gamemode.Avatar.Name} does not enough diamonds. (Diamonds : {gamemode.Avatar.Diamonds}, Require : {cost}).");
                            return;
                        }
                    }

                    gamemode.Avatar.EnergyPackages.AddItem(this.EnergyPackage.GlobalID, 1);
                    gamemode.Avatar.EnergyTimer.Stop();
                    
                    gamemode.Avatar.Diamonds -= cost;
                    gamemode.Avatar.Energy    = gamemode.Avatar.MaxEnergy;
                }
                else Debugger.Error("Unable to buy the energy package. The player has already bought all of the packages.");
            }
            else Debugger.Error("Unable to buy the energy package. The package data does not exist or is invalid.");
        }
    }
}