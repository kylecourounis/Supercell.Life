namespace Supercell.Life.Server.Protocol.Commands.Debugs
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands.Debugs.Chat;

    internal class LogicChatCommands
    {
        internal const string Delimiter = "/";

        private static readonly Dictionary<string, Type> ChatCommands = new Dictionary<string, Type>
        {
            { "rank", typeof(LogicRankCommand) },
            { "resource", typeof(LogicResourcesCommand) }
        };
        
        /// <summary>
        /// Creates the chat command.
        /// </summary>
        internal static LogicChatCommand CreateChatCommand(string type, Connection connection, string[] arguments)
        {
            if (LogicChatCommands.ChatCommands.TryGetValue(type, out Type cType))
            {
                return (LogicChatCommand)Activator.CreateInstance(cType, connection, arguments);
            }

            connection.SendChatMessage(LogicChatCommands.PrintHelp(type));

            return null;
        }

        /// <summary>
        /// Prints the unknown command string.
        /// </summary>
        private static string PrintHelp(string type)
        {
            StringBuilder help = new StringBuilder();

            help.AppendLine($"Failed to handle command : '{type}' is unknown");
            help.AppendLine();
            help.AppendLine("Valid commands are: ");
            foreach (string command in LogicChatCommands.ChatCommands.Keys)
            {
                help.AppendLine($"\t /{command}");
            }

            return help.ToString();
        }
    }
}
