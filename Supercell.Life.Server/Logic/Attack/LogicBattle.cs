namespace Supercell.Life.Server.Logic.Attack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Timers;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    using Timer = System.Timers.Timer;

    internal class LogicBattle
    {
        internal int HighID;
        internal int LowID;

        internal int Tick;

        internal int Seed;

        internal DateTime StartTime;

        internal bool Started;
        internal bool Stopped;

        internal LogicQuestData PvPTier;

        internal Timer Timer;
        internal List<LogicClientAvatar> Avatars;

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
                return this.Avatars.All(avatar => avatar.Battle == null);
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
            this.Timer = new Timer
            {
                AutoReset = true,
                Interval  = 2000
            };
            
            this.StartTime = DateTime.UtcNow;

            this.Seed = Loader.Random.Next(0, 100);

            this.Timer.Elapsed += this.Process;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBattle"/> class.
        /// </summary>
        internal LogicBattle(LogicClientAvatar avatar1, LogicClientAvatar avatar2) : this()
        {
            this.Avatars = new List<LogicClientAvatar>
            {
                avatar1, avatar2
            };

            Debugger.Debug($"{avatar1}, {avatar2}");
        }

        /// <summary>
        /// Encodes this <see cref="LogicBattle"/> using specified stream.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteInt(0);

            stream.WriteBoolean(false);

            stream.WriteDataReference(this.PvPTier);
            stream.WriteDataReference((LogicObstacleData)CSV.Tables.Get(Gamefile.Obstacles).GetDataByName("Bush_A"));
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            if (!this.Started)
            {
                this.Started = true;

                if (!this.Stopped)
                {
                    foreach (LogicClientAvatar avatar in this.Avatars.Where(avatar => avatar.Connection != null))
                    {
                        new StopHomeLogicMessage(avatar.Connection).Send();
                        new SectorStateMessage(avatar.Connection)
                        {
                            Battle = this
                        }.Send();
                    }

                    this.Timer.Start();
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
        /// Processes the specified sender.
        /// </summary>
        private void Process(object sender, ElapsedEventArgs args)
        {
            if (!this.Stopped)
            {
                if (this.Started)
                {
                    int currentSeed = Interlocked.Increment(ref this.Tick);

                    Debugger.Info($"Heartbeat n°{currentSeed}!");

                    foreach (LogicClientAvatar avatar in this.Avatars.Where(avatar => avatar.Connection != null && avatar.Connection.IsConnected))
                    {
                        new SectorHeartbeatMessage(avatar.Connection, currentSeed).Send();
                    }

                    if (this.AllDisconnected)
                    {
                        this.Stop();
                    }
                }
                else
                {
                    Debugger.Error("Battle had not started when Process() was called.");
                }
            }
            else
            {
                Debugger.Error("Battle had already stopped when Process() was called.");
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
                    this.Timer.Stop();

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