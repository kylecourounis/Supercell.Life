namespace Supercell.Life.Server.Logic.Attack
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Protocol.Commands;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    using Timer = System.Timers.Timer;

    internal class LogicBattle
    {
        internal int HighID;
        internal int LowID;

        internal int Seed;

        internal DateTime StartTime;

        internal bool Started;
        internal bool Stopped;

        internal LogicQuestData PvPTier;
        internal LogicEventsData Event;

        internal Timer BattleTimer;
        internal Timer TurnTimer;

        internal List<LogicClientAvatar> Avatars
        {
            get
            {
                return this.CommandQueues.Keys.ToList();
            }
        }

        internal Dictionary<LogicClientAvatar, ConcurrentQueue<LogicCommand>> CommandQueues;
        
        internal int Turn;

        internal int BattleTime => (int)DateTime.UtcNow.Subtract(this.StartTime).TotalSeconds * 2;

        private object Gate;

        /// <summary>
        /// Gets the identifier for this <see cref="LogicBattle"/>.
        /// </summary>
        internal LogicLong Identifier
        {
            get
            {
                return new LogicLong(this.HighID, this.LowID);
            }
        }

        /// <summary>
        /// Gets a value indicating whether all players are disconnected.
        /// </summary>
        internal bool AllDisconnected
        {
            get
            {
                return this.Avatars.All(avatar => avatar.Connection.GameMode.Battle == null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        internal bool IsRunning
        {
            get
            {
                return this.Started && !this.Stopped;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattle"/> class.
        /// </summary>
        internal LogicBattle()
        {
            this.TurnTimer = new Timer
            {
                AutoReset = true,
                Interval  = Globals.PVPFirstTurnTimeSeconds * 1000
            };
            
            this.BattleTimer = new Timer
            {
                AutoReset = true,
                Interval  = 500
            };

            this.BattleTimer.Elapsed += this.Tick;
            this.TurnTimer.Elapsed   += this.SetTurn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattle"/> class.
        /// </summary>
        internal LogicBattle(LogicClientAvatar avatar1, LogicClientAvatar avatar2) : this()
        {
            this.CommandQueues = new Dictionary<LogicClientAvatar, ConcurrentQueue<LogicCommand>>
            {
                { avatar1, new ConcurrentQueue<LogicCommand>() },
                { avatar2, new ConcurrentQueue<LogicCommand>() }
            };
        }

        /// <summary>
        /// Encodes this <see cref="LogicBattle"/> using specified stream.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            stream.WriteInt(Loader.Random.Next(0, 1)); // Coin toss to check who goes first
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteInt(0);

            stream.WriteBoolean(true);

            stream.WriteDataReference(this.PvPTier);
            stream.WriteDataReference(this.Event);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            if (!this.Started)
            {
                this.Started   = true;
                this.StartTime = DateTime.UtcNow;

                if (!this.Stopped)
                {
                    int idx = 0;

                    foreach (LogicClientAvatar avatar in this.Avatars.Where(avatar => avatar.Connection != null))
                    {
                        new StopHomeLogicMessage(avatar.Connection).Send();
                        new SectorStateMessage(avatar.Connection, idx)
                        {
                            Battle = this
                        }.Send();

                        idx++;
                    }

                    this.BattleTimer.Start();
                    this.TurnTimer.Start();
                }
                else
                {
                    Debugger.Error("Battle had already stopped when Start() was called.");
                }
            }
            else
            {
                Debugger.Error("Battle had already started when Start() was called.");
            }
        }

        /// <summary>
        /// Gets the enemy's command queue.
        /// </summary>
        internal ConcurrentQueue<LogicCommand> GetOwnQueue(LogicClientAvatar avatar)
        {
            return this.CommandQueues[this.Avatars.Find(a => a.Identifier == avatar.Identifier)];
        }

        /// <summary>
        /// Gets the enemy's command queue.
        /// </summary>
        internal ConcurrentQueue<LogicCommand> GetEnemyQueue(LogicClientAvatar nonEnemy)
        {
            return this.CommandQueues[this.Avatars.Find(avatar => avatar.Identifier != nonEnemy.Identifier)];
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        private void Tick(object sender, ElapsedEventArgs args)
        {
            if (!this.Stopped)
            {
                if (this.Started)
                {
                    Debugger.Info("Tick.");

                    foreach (LogicClientAvatar avatar in this.Avatars)
                    {
                        new SectorHeartbeatMessage(avatar.Connection)
                        {
                            Commands = this.CommandQueues[avatar],
                            Turn = this.Turn
                        }.Send();
                    }
                    
                    if (this.AllDisconnected)
                    {
                        this.Stop();
                    }
                }
                else
                {
                    Debugger.Error("Battle had not started when Tick() was called.");
                }
            }
            else
            {
                Debugger.Error("Battle had already stopped when Tick() was called.");
            }
        }

        /// <summary>
        /// Sets the turn.
        /// </summary>
        internal void SetTurn(object sender, ElapsedEventArgs args)
        { 
            this.Reset();
        }

        /// <summary>
        /// Resets this the turn timer.
        /// </summary>
        internal void Reset()
        {
            if ((int)this.TurnTimer.Interval == Globals.PVPFirstTurnTimeSeconds * 1000)
            {
                this.TurnTimer.Stop();
                this.TurnTimer.Interval = Globals.PVPMaxTurnTimeSeconds * 1000;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        internal void Stop()
        {
            if (!this.Stopped)
            {
                this.Stopped = true;

                if (this.Started)
                {
                    this.BattleTimer.Stop();
                    this.TurnTimer.Stop();

                    foreach (LogicClientAvatar avatar in this.Avatars)
                    {
                        new BattleResultMessage(avatar.Connection).Send();
                    }

                    Battles.Remove(this);
                }
                else
                {
                    Debugger.Error("Battle hadn't started when Stop() was called");
                }
            }
            else
            {
                Debugger.Error("Battle already stopped when Stop() was called.");
            }
        }
    }
}