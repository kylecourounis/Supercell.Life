namespace Supercell.Life.Server.Core.Consoles
{
    using System;
    using System.Threading;

    using Supercell.Life.Titan.Core;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Core.Events;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class Parser
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Parser"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Parser.Initialized)
            {
                return;
            }

            new Thread(() =>
            {
                while (true)
                {
                    int cursorTop = Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
                    Console.Write($"root@{Constants.LocalIP.ToString().Split(":")[0]} > ");

                    string[] command = Console.ReadLine()?.Split(' ');

                    Console.SetCursorPosition(0, cursorTop - 1);
                    Console.Write(new string(' ', Console.BufferWidth) + Environment.NewLine);
                    Console.SetCursorPosition(0, cursorTop - 2);

                    switch (command?[0].Replace("/", string.Empty))
                    {
                        case "stats":
                        {
                            if (Loader.Initialized)
                            {
                                Console.Write(Environment.NewLine);
                                Console.Write($"#  {DateTime.Now.ToString("d")} ---- STATS ---- {DateTime.Now.ToString("t")} #" + Environment.NewLine);
                                Console.Write("# ----------------------------------- #" + Environment.NewLine);
                                Console.Write($"# Connected Sockets # {LogicStringUtil.IntToString(Connections.Count).Pad(15)} #" + Environment.NewLine);
                                Console.Write($"# In-Memory Avatars # {LogicStringUtil.IntToString(Avatars.Count).Pad(15)} #" + Environment.NewLine);
                                Console.Write($"#  In-Memory Clans  # {LogicStringUtil.IntToString(Alliances.Count).Pad(15)} #" + Environment.NewLine);
                                Console.Write("# ----------------------------------- #" + Environment.NewLine);
                                Console.Write(Environment.NewLine);
                            }

                            break;
                        }

                        case "setrank":
                        {
                            if (Loader.Initialized)
                            {
                                var playerId = command[1].Split('-');
                                var player = Avatars.Get(new LogicLong(LogicStringUtil.ConvertToInt(playerId[0]), LogicStringUtil.ConvertToInt(playerId[1])));

                                var rank = (Rank)Enum.GetValues(typeof(Rank)).GetValue(LogicStringUtil.ConvertToInt(command[2]));

                                player.SetRank(rank);
                            }

                            break;
                        }

                        case "sendDebugCmd":
                        {
                            if (Loader.Initialized && LogicVersion.IsIntegration)
                            {
                                var commandId = LogicStringUtil.ConvertToInt(command[1]);

                                var playerId  = command[2].Split('-');
                                var player    = Avatars.Get(new LogicLong(LogicStringUtil.ConvertToInt(playerId[0]), LogicStringUtil.ConvertToInt(playerId[1])));

                                if (player.Connection != null)
                                {
                                    new AvailableServerCommandMessage(player.Connection, new LogicDebugCommand(player.Connection, commandId)).Send();
                                }
                                    
                                Console.WriteLine($"Sent Debug Command with ID {commandId} to player {player}");
                            }

                            break;
                        }

                        case "sendAllianceStreamEntry":
                        {
                            if (Loader.Initialized)
                            {
                                var type = LogicStringUtil.ConvertToInt(command[1]);

                                var playerId = command[2].Split('-');
                                var player = Avatars.Get(new LogicLong(LogicStringUtil.ConvertToInt(playerId[0]), LogicStringUtil.ConvertToInt(playerId[1])));

                                AllianceMember sender = player.Connection.GameMode.Avatar.Alliance.Members.Find(member => member.Identifier == player.Connection.GameMode.Avatar.Identifier);
                                new AllianceStreamEntryMessage(player.Connection, new AllianceStreamEntry(sender, (AllianceStreamEntry.AllianceStreamType)type)).Send();

                                Console.WriteLine($"Sent AllianceStreamEntry with type {(AllianceStreamEntry.AllianceStreamType)type} ({type}) to player {player}");
                            }

                            break;
                        }

                        case "clear":
                        {
                            Console.Clear();
                            break;
                        }

                        case "exit":
                        case "shutdown":
                        case "stop":
                        {
                            EventsHandler.Exit();
                            break;
                        }

                        default:
                        {
                            Console.WriteLine();
                            break;
                        }
                    }
                }
            }).Start();

            Parser.Initialized = true;
        }
    }
}