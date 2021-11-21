﻿namespace Supercell.Life.Server.Logic.Battle
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Protocol.Commands;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    using Timer = System.Timers.Timer;

    internal class LogicBattle
    {
        internal LogicLong Identifier;

        internal bool Started;

        internal LogicQuestData PvPTier;
        internal LogicEventsData Event;

        internal Timer BattleTick;

        internal int StartingPlayer;

        internal LogicLong WhoseTurn;

        internal LogicMultiplayerTurnTimer TurnTimer;
        
        internal Dictionary<LogicGameMode, ConcurrentQueue<LogicCommand>> CommandQueues;

        internal LogicJSONArray ReplayCommands;

        internal LogicBattleResult BattleResult;

        internal bool IsFriendlyChallenge;

        /// <summary>
        /// Gets a list of the instances of <see cref="LogicGameMode"/>s in this <see cref="LogicBattle"/>.
        /// </summary>
        internal List<LogicGameMode> GameModes
        {
            get
            {
                return this.CommandQueues.Keys.ToList();
            }
        }

        /// <summary>
        /// Gets the number of instances of <see cref="LogicGameMode"/> connected to the <see cref="LogicBattle"/>.
        /// </summary>
        internal List<LogicGameMode> Connected
        {
            get
            {
                return this.GameModes.Where(avatar => avatar.Connection != null).ToList();
            }
        }

        /// <summary>
        /// Gets a value indicating whether all players are disconnected.
        /// </summary>
        internal bool AllDisconnected
        {
            get
            {
                return this.GameModes.All(avatar => avatar.Connection.GameMode.Battle == null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattle"/> class.
        /// </summary>
        internal LogicBattle()
        {
            this.BattleTick = new Timer
            {
                AutoReset = true,
                Interval  = 1000
            };

            this.BattleTick.Elapsed += this.Tick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattle"/> class.
        /// </summary>
        internal LogicBattle(LogicGameMode gamemode1, LogicGameMode gamemode2) : this()
        {
            this.CommandQueues = new Dictionary<LogicGameMode, ConcurrentQueue<LogicCommand>>
            {
                { gamemode1, new ConcurrentQueue<LogicCommand>() },
                { gamemode2, new ConcurrentQueue<LogicCommand>() }
            };

            this.TurnTimer = new LogicMultiplayerTurnTimer(this);

            this.ReplayCommands = new LogicJSONArray();
        }

        /// <summary>
        /// Encodes this <see cref="LogicBattle"/> using specified stream.
        /// </summary>
        internal void Encode(ByteStream stream, LogicGameMode gamemode)
        {
            gamemode.Avatar.Encode(stream);
            this.GetEnemy(gamemode.Avatar).Encode(stream);

            stream.WriteInt(this.WhoseTurn == gamemode.Avatar.Identifier ? 1 : 0); // Coin toss to determine who goes first
            
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteInt(0);

            stream.WriteBoolean(this.IsFriendlyChallenge);

            stream.WriteDataReference(this.PvPTier);
            stream.WriteDataReference(this.Event);
        }

        /// <summary>
        /// Gets the enemy of the specified <see cref="LogicClientAvatar"/>.
        /// </summary>
        internal LogicClientAvatar GetEnemy(LogicClientAvatar avatar)
        {
            return this.GameModes.Find(gamemode => gamemode.Avatar.Identifier != avatar.Identifier).Avatar;
        }
        
        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            if (!this.Started)
            {
                this.Started = true;

                this.StartingPlayer = Loader.Random.Next(0, 1);
                this.WhoseTurn      = this.GameModes[this.StartingPlayer].Avatar.Identifier;

                foreach (LogicGameMode gamemode in this.GameModes.Where(gamemode => gamemode.Connection != null))
                {
                    new StopHomeLogicMessage(gamemode.Connection).Send();
                    new SectorStateMessage(gamemode.Connection)
                    {
                        Battle = this
                    }.Send();
                }

                this.TurnTimer.Start();
                this.BattleTick.Start();
            }
            else
            {
                Debugger.Error("Battle had already started when Start() was called.");
            }
        }

        /// <summary>
        /// Adds the specified <see cref="LogicCommand"/> to both queues. 
        /// </summary>
        internal void EnqueueCommand(LogicCommand self, LogicCommand opponent)
        {
            if (self != null)
            {
                this.CommandQueues[this.GameModes.Find(gamemode => gamemode.Avatar.Identifier == self.Connection.GameMode.Avatar.Identifier)].Enqueue(self);
                this.ReplayCommands.Add(self.SaveToJSON());
            }

            if (opponent != null)
            {
                this.CommandQueues[this.GameModes.Find(gamemode => gamemode.Avatar.Identifier != self.Connection.GameMode.Avatar.Identifier)].Enqueue(opponent);
            }
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        private void Tick(object sender, ElapsedEventArgs args)
        {
            if (this.Started)
            {
                this.TurnTimer.Tick();

                if (this.AllDisconnected)
                {
                    this.Stop();
                }
                else
                {
                    foreach (LogicGameMode gamemode in this.Connected)
                    {
                        new SectorHeartbeatMessage(gamemode.Connection)
                        {
                            Commands = this.CommandQueues[gamemode]
                        }.Send();
                    }

                    if (this.GameModes.Where(gamemode => gamemode.Resigned).ToList().Count > 0)
                    {
                        this.Stop();
                    }

                    if (this.Connected.Count == 1)
                    {
                        this.TurnTimer.EnemyReconnectTurns++;

                        if (this.TurnTimer.EnemyReconnectTurns == 3)
                        {
                            this.Stop();
                        }
                    }
                }
            }
            else
            {
                Debugger.Error("Battle had not started when Tick() was called.");
            }
        }

        /// <summary>
        /// Resets this the turn timer.
        /// </summary>
        internal void ResetTurn(LogicClientAvatar turnFinished)
        {
            this.WhoseTurn = this.GetEnemy(turnFinished).Identifier;

            this.TurnTimer.Reset();

            Debugger.Debug($"{this.WhoseTurn} turn.");
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        internal void Stop()
        {
            if (this.Started)
            {
                this.TurnTimer.Finish();
                this.BattleTick.Stop();
                
                this.BattleResult = new LogicBattleResult(this);

                this.BattleResult.Determine();
                this.BattleResult.Send();

                Battles.Remove(this);
            }
            else
            {
                Debugger.Error("Battle hadn't started when Stop() was called");
            }
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONObject"/> for this <see cref="LogicBattle"/>.
        /// </summary>
        internal LogicJSONObject JSON
        {
            get
            {
                LogicJSONObject json = new LogicJSONObject();

                // TODO

                return json;
            }
        }
    }
}