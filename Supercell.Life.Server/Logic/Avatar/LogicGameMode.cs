namespace Supercell.Life.Server.Logic.Avatar
{
    using System;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects;
    using Supercell.Life.Server.Network;

    internal class LogicGameMode
    {
        internal Connection Connection;

        internal bool Resigned;

        internal LogicBattle Battle;


        internal LogicClientAvatar Avatar
        {
            get
            {
                return this.Connection.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicGameMode"/> class.
        /// </summary>
        internal LogicGameMode(Connection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Called when the player wins a battle.
        /// </summary>
        internal void WinBattle(int trophies = 30)
        {
            LogicClientAvatar avatar = this.Connection.Avatar;

            LogicLeagueData league = (LogicLeagueData)CSV.Tables.Get(Gamefile.Leagues).GetDataWithID(avatar.League);

            avatar.AddGold(league.PVPGoldReward);
            avatar.AddXP(league.PVPXpReward);
            avatar.AddTrophies(trophies);

            avatar.Variables.AddItem(LogicVariables.Wins.GlobalID, 1);
            avatar.Variables.AddItem(LogicVariables.WinStreak.GlobalID, 1);
            avatar.Variables.AddItem(LogicVariables.Matches.GlobalID, 1);

            avatar.Variables.AddItem(LogicVariables.ChestProgress.GlobalID, 1);
            avatar.Variables.AddItem(LogicVariables.ChestProgressUpdated.GlobalID, 1);

            if (avatar.Variables.Get(LogicVariables.ChestProgress.GlobalID).Count == 5 && avatar.Variables.Get(LogicVariables.ChestProgressUpdated.GlobalID).Count == 5)
            {
                avatar.Variables.Set(LogicVariables.ChestProgress.GlobalID, 0);
                avatar.Variables.Set(LogicVariables.ChestProgressUpdated.GlobalID, 0);

                LogicChest chest = new LogicChest(avatar);
                chest.CreateMegaChest();
            }
        }

        /// <summary>
        /// Called when the player loses a battle.
        /// </summary>
        internal void LoseBattle()
        {
            this.Connection.Avatar.LoseTrophies(30);
            this.Connection.Avatar.GameMode.Resigned = false;
        }


        /// <summary>
        /// Adjusts the sub tick.
        /// </summary>
        internal void AdjustSubTick()
        {
            this.Avatar.EnergyTimer.AdjustSubTick();
            this.Avatar.HeroUpgrade.AdjustSubTick();
            this.Avatar.Sailing.AdjustSubTick();
            this.Avatar.ShipUpgrade.AdjustSubTick();
            this.Avatar.Booster.AdjustSubTick();
            this.Avatar.SpellTimer.AdjustSubTick();
            this.Avatar.ItemUnavailableTimer.AdjustSubTick();

            if (this.Avatar.Alliance != null)
            {
                this.Avatar.Alliance.TeamGoalTimer.AdjustSubTick();

                if (!this.Avatar.Alliance.TeamGoalTimer.Started)
                {
                    this.Avatar.Alliance.TeamGoalTimer.Start();
                }
            }
        }

        /// <summary>
        /// Fast forwards this instance.
        /// </summary>
        internal void FastForward(int seconds)
        {
            this.Avatar.EnergyTimer.FastForward(seconds);
            this.Avatar.HeroUpgrade.FastForward(seconds);
            this.Avatar.Sailing.FastForward(seconds);
            this.Avatar.ShipUpgrade.FastForward(seconds);
            this.Avatar.Booster.FastForward(seconds);
            this.Avatar.SpellTimer.FastForward(seconds);
            this.Avatar.ItemUnavailableTimer.FastForward(seconds);
            this.Avatar.Alliance?.TeamGoalTimer.FastForward(seconds);

            this.AdjustSubTick();
            this.Avatar.Save();
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            this.Avatar.EnergyTimer.Tick();
            this.Avatar.HeroUpgrade.Tick();
            this.Avatar.Sailing.Tick();
            this.Avatar.ShipUpgrade.Tick();
            this.Avatar.Booster.Tick();
            this.Avatar.SpellTimer.Tick();
            this.Avatar.ItemUnavailableTimer.Tick();
            this.Avatar.Alliance?.TeamGoalTimer.Tick();

            Debugger.Debug($"Energy Timer           : Started: {this.Avatar.EnergyTimer.Timer.Started}  : RemainingSecs: {this.Avatar.EnergyTimer.Timer.RemainingSecs}.");
            Debugger.Debug($"Hero Upgrade Timer     : Started: {this.Avatar.HeroUpgrade.Timer.Started}  : RemainingSecs: {this.Avatar.HeroUpgrade.Timer.RemainingSecs}.");
            Debugger.Debug($"Sailing Timer          : Started: {this.Avatar.Sailing.Timer.Started}  : RemainingSecs: {this.Avatar.Sailing.Timer.RemainingSecs}.");
            Debugger.Debug($"Ship Upgrade Timer     : Started: {this.Avatar.ShipUpgrade.Timer.Started}  : RemainingSecs: {this.Avatar.ShipUpgrade.Timer.RemainingSecs}.");
            Debugger.Debug($"XP Booster Timer       : Started: {this.Avatar.Booster.Timer.Started}  : RemainingSecs: {this.Avatar.Booster.Timer.RemainingSecs}.");
            Debugger.Debug($"Spell Timer            : Started: {this.Avatar.SpellTimer.Started}  : RemainingSecs: {this.Avatar.SpellTimer.Timer.RemainingSecs}.");
            Debugger.Debug($"Item Unavailable Timer : Started: {this.Avatar.ItemUnavailableTimer.Started}  : RemainingSecs: {this.Avatar.ItemUnavailableTimer.Timer.RemainingSecs}.");
            Debugger.Debug($"Team Goal Timer        : Started: {this.Avatar.Alliance?.TeamGoalTimer.Started}  : RemainingSecs: {this.Avatar.Alliance?.TeamGoalTimer.Timer.RemainingSecs}.");

            this.Avatar.Update = DateTime.UtcNow;
        }
    }
}
