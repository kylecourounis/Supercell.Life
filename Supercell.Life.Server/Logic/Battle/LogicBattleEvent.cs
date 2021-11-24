namespace Supercell.Life.Server.Logic.Battle
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Enums;

    internal class LogicBattleEvent
    {
        internal readonly LogicBattle Battle;
        internal readonly BattleEvent EventType;

        internal int PlayerIndex, TauntIndex;
        internal int DirectionX, DirectionY;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattleEvent"/> class.
        /// </summary>
        internal LogicBattleEvent(LogicBattle battle, BattleEvent eventType, int value1, int value2)
        {
            this.Battle    = battle;
            this.EventType = eventType;

            switch (eventType)
            {
                case BattleEvent.Taunt:
                {
                    this.PlayerIndex = value1;
                    this.TauntIndex  = value2;

                    break;
                }
                case BattleEvent.Aim:
                {
                    this.DirectionX = -value1;
                    this.DirectionY = -value2;

                    break;
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt((int)this.EventType);

            switch (this.EventType)
            {
                case BattleEvent.Taunt:
                {
                    encoder.WriteInt(this.PlayerIndex == 0 ? 1 : 0); // Invert the Player Index so that the taunt bubble will actually show
                    encoder.WriteInt(this.TauntIndex);

                    break;
                }
                case BattleEvent.Aim:
                {
                    encoder.WriteInt(this.DirectionX);
                    encoder.WriteInt(this.DirectionY);

                    break;
                }
            }
        }
    }
}
