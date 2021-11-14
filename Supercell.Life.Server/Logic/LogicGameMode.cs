namespace Supercell.Life.Server.Logic
{
    using System;

    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol;
    using Supercell.Life.Server.Protocol.Commands;

    internal class LogicGameMode
    {
        internal Connection Connection;

        internal readonly MessageManager MessageManager;
        internal readonly LogicCommandManager CommandManager;

        internal LogicClientAvatar Avatar;

        internal LogicBattle Battle;

        internal bool Resigned;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicGameMode"/> class.
        /// </summary>
        internal LogicGameMode(Connection connection)
        {
            this.Connection     = connection;
            this.MessageManager = new MessageManager(connection);
            this.CommandManager = new LogicCommandManager(connection);
        }

        /// <summary>
        /// Loads a <see cref="LogicReplay"/> from the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal LogicReplay LoadReplay(LogicJSONObject json)
        {
            return new LogicReplay(this)
            {
                Quest          = (LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataWithID(json.GetJsonObject("quest").GetJsonNumber("quest_data").GetIntValue()),
                Event          = (LogicEventsData)CSV.Tables.Get(Gamefile.Events).GetDataWithID(json.GetJsonNumber("event_data").GetIntValue()),
                LevelIndex     = json.GetJsonNumber("level_idx").GetIntValue(),
                Challenge      = json.GetJsonNumber("challenge").GetIntValue(),
                StartingPlayer = json.GetJsonNumber("starting_player").GetIntValue(),
                Avatar         = json.GetJsonObject("avatar"),
                Avatar2        = json.GetJsonObject("avatar2"),
                EndTick        = json.GetJsonNumber("end_tick").GetIntValue(),
                Commands       = LogicReplay.GetCommands(json.GetJsonArray("cmd"), this.Connection)
            };
        }

        /// <summary>
        /// Saves a <see cref="LogicReplay"/> to the returned <see cref="LogicJSONObject"/>.
        /// </summary>
        internal LogicJSONObject SaveReplay()
        {
            LogicJSONObject replayJSON = new LogicJSONObject();

            var quest = new LogicJSONObject();
            quest.Put("quest_data", new LogicJSONNumber(this.Battle.PvPTier.GlobalID));
            
            replayJSON.Put("quest", quest);
            replayJSON.Put("event_data", new LogicJSONNumber(this.Battle.Event?.GlobalID ?? 0));

            replayJSON.Put("level_idx", new LogicJSONNumber());
            replayJSON.Put("challenge", new LogicJSONNumber());
            replayJSON.Put("starting_player", new LogicJSONNumber());

            replayJSON.Put("avatar", this.Battle.Avatars[0].GetAvatarJSON());
            replayJSON.Put("avatar2", this.Battle.Avatars[1].GetAvatarJSON());

            replayJSON.Put("end_tick", new LogicJSONNumber());
            replayJSON.Put("cmd", this.Battle.ReplayCommands);

            return replayJSON;
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
