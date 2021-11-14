namespace Supercell.Life.Server.Logic.Attack
{
    using System;
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
        internal int HighID;
        internal int LowID;

        internal int Seed;

        internal DateTime StartTime;

        internal bool Started;
        internal bool Stopped;

        internal LogicQuestData PvPTier;
        internal LogicEventsData Event;

        internal Timer BattleTick;

        internal List<LogicClientAvatar> Avatars
        {
            get
            {
                return this.CommandQueues.Keys.ToList();
            }
        }

        internal int StartingPlayer;

        internal LogicLong WhoseTurn;

        internal Dictionary<LogicClientAvatar, ConcurrentQueue<LogicCommand>> CommandQueues;

        internal LogicJSONArray ReplayCommands;

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
            this.BattleTick = new Timer
            {
                AutoReset = true,
                Interval  = 500
            };

            this.BattleTick.Elapsed += this.Tick;
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

            this.ReplayCommands = new LogicJSONArray();
        }

        /// <summary>
        /// Encodes this <see cref="LogicBattle"/> using specified stream.
        /// </summary>
        internal void Encode(ByteStream stream, int side)
        {
            switch (side) 
            {
                case 0:
                {
                    this.Avatars[0].Encode(stream);
                    this.Avatars[1].Encode(stream);

                    stream.WriteInt(this.StartingPlayer == 0 ? 1 : 0); // Coin toss to determine who goes first

                    break;
                }
                case 1:
                {
                    this.Avatars[1].Encode(stream);
                    this.Avatars[0].Encode(stream);

                    stream.WriteInt(this.StartingPlayer == 1 ? 1 : 0);

                    break;
                }
            }

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
                    int side = 0;

                    this.StartingPlayer = Loader.Random.Next(0, 1);

                    foreach (LogicClientAvatar avatar in this.Avatars.Where(avatar => avatar.Connection != null))
                    {
                        new StopHomeLogicMessage(avatar.Connection).Send();
                        new SectorStateMessage(avatar.Connection, side)
                        {
                            Battle = this
                        }.Send();

                        side++;
                    }

                    this.BattleTick.Start();
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
        /// Adds the specified <see cref="LogicCommand"/> to both queues. 
        /// </summary>
        internal void EnqueueCommand(LogicCommand self, LogicCommand opponent)
        {
            if (self != null)
            {
                this.GetOwnQueue(self.Connection.GameMode.Avatar).Enqueue(self);
                this.ReplayCommands.Add(self.SaveToJSON());
            }

            if (opponent != null)
            {
                this.GetEnemyQueue(self.Connection.GameMode.Avatar).Enqueue(opponent);
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
                            Commands = this.CommandQueues[avatar]
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
        /// Resets this the turn timer.
        /// </summary>
        internal void ResetTurn(LogicClientAvatar turnFinished)
        {
            this.WhoseTurn = this.Avatars.Find(avatar => avatar.Identifier != turnFinished.Identifier).Identifier;

            Debugger.Debug($"{this.WhoseTurn} turn.");
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
                    this.BattleTick.Stop();

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