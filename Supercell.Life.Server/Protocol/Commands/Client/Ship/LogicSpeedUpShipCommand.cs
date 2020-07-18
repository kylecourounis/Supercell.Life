namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicSpeedUpShipCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpeedUpShipCommand"/> class.
        /// </summary>
        public LogicSpeedUpShipCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicSpeedUpShipCommand.
        }

        internal override void Decode()
        {
            var cost = LogicGamePlayUtil.GetSpeedUpCost(this.Connection.Avatar.Sailing.Timer.RemainingSecs, LogicGamePlayUtil.GetSpeedUpCostMultiplier(4));
            Debugger.Debug(cost);

            this.ReadHeader();
        }

        internal override void Execute()
        {
            var cost = LogicGamePlayUtil.GetSpeedUpCost(this.Connection.Avatar.Sailing.Timer.RemainingSecs, LogicGamePlayUtil.GetSpeedUpCostMultiplier(4));
            Debugger.Debug(cost);
            
            this.Connection.Avatar.Diamonds -= cost;

            this.Connection.Avatar.Sailing.Finish();
            this.Connection.Avatar.Save();
        }
        
        private int CalculateSpeedUpCost()
        {
            double cost = LogicGamePlayUtil.GetSpeedUpCost(this.Connection.Avatar.Sailing.Timer.RemainingSecs, LogicGamePlayUtil.GetSpeedUpCostMultiplier(4));

            if (this.Connection.Avatar.Sailing.Heroes.Count == 2)
            {
                cost += (cost / 2);
            }
            else if (this.Connection.Avatar.Sailing.Heroes.Count == 3)
            {
                cost = (cost * 2) + 1;
            }

            Debugger.Debug(Math.Round(cost));

            return (int)Math.Round(cost);
        }
    }
}
