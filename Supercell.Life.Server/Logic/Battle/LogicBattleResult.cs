namespace Supercell.Life.Server.Logic.Battle
{
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicBattleResult
    {
        internal LogicBattle Battle;

        internal LogicGameMode Winner, Loser;

        internal int WinnerScore, LoserScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattleResult"/> class.
        /// </summary>
        internal LogicBattleResult(LogicBattle battle)
        {
            this.Battle = battle;
        }

        /// <summary>
        /// Gets a value indicating whether the battle ended because one of the players resigned.
        /// </summary>
        private bool Resignation
        {
            get
            {
                return this.Battle.GameModes.FindAll(gamemode => gamemode.Resigned).Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the battle ended because one of the players disconnected.
        /// </summary>
        private bool PlayerDisconnected
        {
            get
            {
                return this.Battle.TurnTimer.EnemyReconnectTurns.Turns == 3;
            }
        }

        /// <summary>
        /// Determines the results of the battle.
        /// </summary>
        internal void Determine()
        {
            if (this.Resignation)
            {
                this.SetWinnerAndLoser(this.Battle.GameModes.Find(gamemode => !gamemode.Resigned), this.Battle.GameModes.Find(gamemode => gamemode.Resigned));
            }
            else if (this.PlayerDisconnected)
            {
                var winner = this.Battle.Connected[0];
                this.SetWinnerAndLoser(winner, this.Battle.GameModes.Find(gamemode => gamemode.Avatar.Identifier != winner.Avatar.Identifier));
            }
            else
            {
                // TODO
            }

            (this.WinnerScore, this.LoserScore) = LogicGamePlayUtil.CalculateScore(this.Winner.Avatar, this.Loser.Avatar);

            Debugger.Debug($"{this.WinnerScore}, {this.LoserScore}");

            // this.UpdateScores();
        }

        /// <summary>
        /// Updates the trophy count of both the winner and the loser. 
        /// </summary>
        private void UpdateScores()
        {
            (this.WinnerScore, this.LoserScore) = LogicGamePlayUtil.CalculateScore(this.Winner.Avatar, this.Loser.Avatar);

            this.Winner.Avatar.AddTrophyScoreHelper(this.WinnerScore);
            this.Loser.Avatar.AddTrophyScoreHelper(this.LoserScore);

            this.Winner.Avatar.WinBattle();
            this.Loser.Avatar.LoseBattle();
        }

        /// <summary>
        /// Sets the winner and the loser.
        /// </summary>
        private void SetWinnerAndLoser(LogicGameMode winner, LogicGameMode loser)
        {
            this.Winner = winner;
            this.Loser  = loser;

            foreach (var hero in this.Winner.Avatar.Team.ToObject<int[]>())
            {
                this.Winner.Avatar.DailyMultiplayerTimer.HeroUsedInDaily.Add(hero);
            }
        }

        /// <summary>
        /// Sends the <see cref="BattleResultMessage"/> to the instances of <see cref="LogicGameMode"/> in the battle.
        /// </summary>
        internal void Send()
        {
            this.Battle.GameModes.ForEach(gamemode =>
            {
                new BattleResultMessage(gamemode.Connection)
                {
                    BattleResult = this
                }.Send();

                gamemode.Battle = null;
            });
        }
    }
}
