namespace Supercell.Life.Server.Logic.Battle
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;
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

        internal DateTime StartTime;

        internal bool Started;
        internal bool Stopped;

        internal LogicQuestData PvPTier;
        internal LogicEventsData Event;

        internal Timer BattleTick;
        
        internal int StartingPlayer;

        internal LogicLong WhoseTurn;

        internal Dictionary<LogicGameMode, ConcurrentQueue<LogicCommand>> CommandQueues;

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
        /// Gets a list of the instances of <see cref="LogicGameMode"/>s in this <see cref="LogicBattle"/>.
        /// </summary>
        internal LogicArrayList<LogicGameMode> GameModes
        {
            get
            {
                return (LogicArrayList<LogicGameMode>)this.CommandQueues.Keys.ToList();
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
        internal LogicBattle(LogicGameMode gamemode1, LogicGameMode gamemode2) : this()
        {
            this.CommandQueues = new Dictionary<LogicGameMode, ConcurrentQueue<LogicCommand>>
            {
                { gamemode1, new ConcurrentQueue<LogicCommand>() },
                { gamemode2, new ConcurrentQueue<LogicCommand>() }
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
                    this.GameModes[0].Avatar.Encode(stream);
                    this.GameModes[1].Avatar.Encode(stream);

                    stream.WriteInt(this.StartingPlayer == 0 ? 1 : 0); // Coin toss to determine who goes first

                    break;
                }
                case 1:
                {
                    this.GameModes[1].Avatar.Encode(stream);
                    this.GameModes[0].Avatar.Encode(stream);

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
        /// Gets the enemy of the specified <see cref="LogicClientAvatar"/>.
        /// </summary>
        internal LogicClientAvatar GetEnemy(LogicClientAvatar avatar)
        {
            return this.GameModes.Find(gamemode => gamemode.Avatar.Identifier != avatar.Identifier).Avatar;
        }

        /// <summary>
        /// Gets the enemy's command queue.
        /// </summary>
        internal ConcurrentQueue<LogicCommand> GetOwnQueue(LogicClientAvatar avatar)
        {
            return this.CommandQueues[this.GameModes.Find(gamemode => gamemode.Avatar.Identifier == avatar.Identifier)];
        }

        /// <summary>
        /// Gets the enemy's command queue.
        /// </summary>
        internal ConcurrentQueue<LogicCommand> GetEnemyQueue(LogicClientAvatar nonEnemy)
        {
            return this.CommandQueues[this.GameModes.Find(gamemode => gamemode.Avatar.Identifier != nonEnemy.Identifier)];
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

                    foreach (LogicGameMode gamemode in this.GameModes.Where(gamemode => gamemode.Connection != null))
                    {
                        new StopHomeLogicMessage(gamemode.Connection).Send();
                        new SectorStateMessage(gamemode.Connection, side)
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
        /// Ticks this instance.
        /// </summary>
        private void Tick(object sender, ElapsedEventArgs args)
        {
            if (!this.Stopped)
            {
                if (this.Started)
                {
                    Debugger.Info("Tick.");

                    foreach (LogicGameMode gamemode in this.GameModes)
                    {
                        new SectorHeartbeatMessage(gamemode.Connection)
                        {
                            Commands = this.CommandQueues[gamemode]
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
            this.WhoseTurn = this.GetEnemy(turnFinished).Identifier;

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

                    foreach (LogicGameMode gamemode in this.GameModes)
                    {
                        new BattleResultMessage(gamemode.Connection).Send();
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