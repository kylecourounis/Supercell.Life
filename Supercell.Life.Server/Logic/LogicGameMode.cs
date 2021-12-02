namespace Supercell.Life.Server.Logic
{
    using System;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Battle;
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

        internal LogicRandom Random;

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
            this.Random         = new LogicRandom();
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
            replayJSON.Put("starting_player", new LogicJSONNumber(this.Battle.StartingPlayer));

            replayJSON.Put("avatar", this.Battle.GameModes[0].Avatar.GetAvatarJSON());
            replayJSON.Put("avatar2", this.Battle.GameModes[1].Avatar.GetAvatarJSON());

            var replayCommands  = this.Battle.ReplayCommands;
            var lastExecuteTick = replayCommands.GetJsonObject(replayCommands.Size - 1).GetJsonObject("c").GetJsonNumber("t").GetIntValue();

            replayJSON.Put("end_tick", new LogicJSONNumber(lastExecuteTick));
            replayJSON.Put("cmd", replayCommands);

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
            this.Avatar.HeroTiredTimer.AdjustSubTick();
            this.Avatar.ShipUpgrade.AdjustSubTick();
            this.Avatar.Booster.AdjustSubTick();
            this.Avatar.SpellTimer.AdjustSubTick();
            this.Avatar.ItemUnavailableTimer.AdjustSubTick();
            this.Avatar.BonusChestRespawnTimer.AdjustSubTick();
            this.Avatar.DailyMultiplayerTimer.AdjustSubTick();

            if (this.Avatar.Alliance != null)
            {
                if (!this.Avatar.Alliance.TeamGoalTimer.Started)
                {
                    this.Avatar.Alliance.TeamGoalTimer.Start();
                }

                this.Avatar.Alliance.TeamGoalTimer.AdjustSubTick();
                this.Avatar.TeamMailCooldownTimer.AdjustSubTick();
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
            this.Avatar.HeroTiredTimer.FastForward(seconds);
            this.Avatar.ShipUpgrade.FastForward(seconds);
            this.Avatar.Booster.FastForward(seconds);
            this.Avatar.SpellTimer.FastForward(seconds);
            this.Avatar.ItemUnavailableTimer.FastForward(seconds);
            this.Avatar.BonusChestRespawnTimer.FastForward(seconds);
            this.Avatar.DailyMultiplayerTimer.FastForward(seconds);

            if (this.Avatar.Alliance != null)
            {
                this.Avatar.Alliance.TeamGoalTimer.FastForward(seconds);
                this.Avatar.TeamMailCooldownTimer.FastForward(seconds);
            }

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
            this.Avatar.HeroTiredTimer.Tick();
            this.Avatar.ShipUpgrade.Tick();
            this.Avatar.Booster.Tick();
            this.Avatar.SpellTimer.Tick();
            this.Avatar.ItemUnavailableTimer.Tick();
            this.Avatar.BonusChestRespawnTimer.Tick();
            this.Avatar.DailyMultiplayerTimer.Tick();

            Debugger.Debug($"Energy Timer           : Started: {this.Avatar.EnergyTimer.Timer.Started}  : RemainingSecs: {this.Avatar.EnergyTimer.Timer.RemainingSecs}.");
            
            if (this.Avatar.Alliance != null)
            {
                this.Avatar.Alliance.TeamGoalTimer.Tick();
                this.Avatar.TeamMailCooldownTimer.Tick();
            }

            this.Connection.GameMode.Random.SetIteratedRandomSeed(LogicTime.GetSecondsInTicks(this.Avatar.Time.TotalSecs));

            this.Avatar.Update = DateTime.UtcNow;
        }
    }
}
