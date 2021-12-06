namespace Supercell.Life.Server.Logic.Battle
{
    using System.Collections.Generic;

    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Game.Objects;

    internal class LogicTurnHandler
    {
        internal LogicBattle Battle;

        internal LogicGameMode WhoseTurn;

        internal int CharacterIndex;

        internal Dictionary<LogicGameMode, LogicCharacter[]> Heroes;

        internal LogicMultiplayerTurnTimer Timer;

        internal int TotalTurns;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTurnHandler"/> class.
        /// </summary>
        internal LogicTurnHandler(LogicBattle battle)
        {
            this.Battle = battle;
            this.Timer  = new LogicMultiplayerTurnTimer(this.Battle);
            this.Heroes = new Dictionary<LogicGameMode, LogicCharacter[]>();
        }

        /// <summary>
        /// Sets the character positions on the battlefield. 
        /// </summary>
        internal void SetCharacterPosition(LogicVector2 position)
        {
            this.Heroes[this.WhoseTurn][this.CharacterIndex].Position = position;
        }

        /// <summary>
        /// Sets the character positions on the battlefield.
        /// Still need to figure out the default positions on the screen.
        /// </summary>
        internal void SetCharacterPositions(LogicGameMode gamemode, LogicVector2 char1, LogicVector2 char2, LogicVector2 char3)
        {
            this.Heroes[gamemode][0].Position = char1;
            this.Heroes[gamemode][1].Position = char2;
            this.Heroes[gamemode][2].Position = char3;
        }

        /// <summary>
        /// Resets this the turn.
        /// </summary>
        internal void ResetTurn(LogicClientAvatar turnFinished)
        {
            this.WhoseTurn = this.Battle.GetEnemy(turnFinished).Connection.GameMode;

            this.TotalTurns++;

            this.Timer.Reset();

            Debugger.Debug($"{this.WhoseTurn.Avatar.Identifier} turn.");
        }
    }
}
