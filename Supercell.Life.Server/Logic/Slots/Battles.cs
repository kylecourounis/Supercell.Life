namespace Supercell.Life.Server.Logic.Slots
{
    using System.Collections.Concurrent;
    using System.Threading;

    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Titan.Logic.Math;

    internal static class Battles
    {
        private static ConcurrentDictionary<LogicLong, LogicBattle> Pool;

        private static int Seed;

        /// <summary>
        /// Gets a value indicating whether <see cref="Battles"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Battles"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Battles.Initialized)
            {
                return;
            }

            Battles.Pool = new ConcurrentDictionary<LogicLong, LogicBattle>();
            Battles.Seed = 0;

            Waiting.Init();

            Battles.Initialized = true;
        }

        /// <summary>
        /// Adds the specified battle.
        /// </summary>
        internal static void Add(LogicBattle battle)
        {
            if (battle.LowID == 0)
            {
                battle.LowID = Interlocked.Increment(ref Battles.Seed);
            }

            if (Battles.Pool.ContainsKey(battle.Identifier))
            {
                Debugger.Warning("This battle is already in the pool!");

                if (!Battles.Pool.TryUpdate(battle.Identifier, battle, battle))
                {
                    Debugger.Error("Failed to update the battle in the pool.");
                }
            }
            else
            {
                if (!Battles.Pool.TryAdd(battle.Identifier, battle))
                {
                    Debugger.Error("Failed to add the battle to the pool.");
                }
            }
        }

        /// <summary>
        /// Removes the specified battle.
        /// </summary>
        internal static void Remove(LogicBattle battle)
        {
            if (Battles.Pool.ContainsKey(battle.Identifier))
            {
                if (Battles.Pool.TryRemove(battle.Identifier, out LogicBattle tmpBattle))
                {
                    if (battle.Identifier != tmpBattle.Identifier)
                    {
                        Debugger.Error("The returned TmpBattle is not equal to our Battle at Remove(Battle).");
                    }
                }
                else
                {
                    Debugger.Error("TryRemove returned false.");
                }
            }
            else
            {
                Debugger.Error("Battle pool does not contain the key.");
            }
        }

        /// <summary>
        /// Gets the battle using the specified battle identifier.
        /// </summary>
        internal static LogicBattle Get(LogicLong id)
        {
            if (Battles.Pool.ContainsKey(id))
            {
                if (Battles.Pool.TryGetValue(id, out LogicBattle battle))
                {
                    if (battle.Identifier == id)
                    {
                        return battle;
                    }

                    Debugger.Error("The returned battle identifier is not equal to the one we requested.");
                }
                else
                {
                    Debugger.Warning("TryGetValue returned false.");
                }
            }
            else
            {
                Debugger.Warning("Battle pool does not contain the key.");
            }

            return null;
        }
    }
}