namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicDebugCommand : LogicServerCommand
    {
        internal int Debug;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDebugCommand"/> class.
        /// </summary>
        public LogicDebugCommand(Connection connection, int debug) : base(connection)
        {
            this.Type  = Command.Debug;
            this.Debug = debug;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.Debug);
            encoder.WriteInt(0);
            encoder.WriteInt(0);

            this.WriteHeader(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            switch (this.Debug)
            {
                case 15:
                {
                    gamemode.FastForward(60);
                    Debugger.Info("Fast forward 1 minute");

                    break;
                }
                case 16:
                {
                    gamemode.FastForward(3600);
                    Debugger.Info("Fast forward 1 hour");

                    break;
                }
                case 17:
                {
                    gamemode.FastForward(86400);
                    Debugger.Info("Fast forward 24 hours");

                    break;
                }
                case 20:
                {
                    gamemode.Avatar.EnergyTimer.Stop();
                    gamemode.Avatar.Energy = gamemode.Avatar.MaxEnergy;

                    Debugger.Info("Energy filled");

                    break;
                }
                case 21:
                case 22:
                case 23:
                case 24:
                {
                    int v44 = 100;

                    switch (this.Debug)
                    {
                        case 21:
                            v44 = -100;
                            break;
                        case 22:
                            v44 = 1000;
                            break;
                        case 23:
                            v44 = -1000;
                            break;
                    }
                        
                    Debugger.Info($"Change {gamemode.Avatar.Score} trophy score [new {gamemode.Avatar.Score + v44}]");

                    gamemode.Avatar.AddTrophies(v44);
                        
                    break;
                }
                case 25:
                case 26:
                case 36:
                case 37:
                {
                    int v35 = 100;
                    
                    switch (this.Debug)
                    {
                        case 26:
                            v35 = -100;
                            break;
                        case 36:
                            v35 = 1000;
                            break;
                        case 37:
                            v35 = -1000;
                            break;
                    }

                    Debugger.Info($"Change {gamemode.Avatar.Diamonds} diamond count [new {gamemode.Avatar.Diamonds + v35}]");

                    new AvailableServerCommandMessage(this.Connection, new LogicDiamondsAddedCommand(this.Connection)
                    {
                        Diamonds = v35
                    }).Send();

                    gamemode.Avatar.CommodityChangeCountHelper(LogicCommodityType.Diamonds, v35);
                        
                    break;
                }
            }
        }
    }
}
