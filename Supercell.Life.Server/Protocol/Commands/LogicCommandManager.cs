namespace Supercell.Life.Server.Protocol.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Client;
    using Supercell.Life.Server.Protocol.Commands.Debugs.Chat;

    internal class LogicCommandManager
    {
        internal const string Delimiter = "/";

        private static readonly Dictionary<string, Type> ChatCommands = new Dictionary<string, Type>();

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicCommandManager"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="LogicCommandManager"/> class.
        /// </summary>
        internal static void Init()
        {
            if (LogicCommandManager.Initialized)
            {
                return;
            }

            LogicCommandManager.ChatCommands.Add("rank", typeof(LogicRankCommand));
            LogicCommandManager.ChatCommands.Add("resource", typeof(LogicResourcesCommand));

            LogicCommandManager.Initialized = true;
        }

        /// <summary>
        /// Creates a <see cref="LogicCommand"/> using the specified type.
        /// </summary>
        internal static LogicCommand CreateCommand(int type, Connection connection, ByteStream stream)
        {
            switch ((Command)type)
            {
                case Command.StartQuest:
                {
                    return new LogicStartQuestCommand(connection, stream);
                }
                case Command.BuyHero:
                {
                    return new LogicBuyHeroCommand(connection, stream);
                }
                case Command.UpgradeHero:
                {
                    return new LogicUpgradeHeroCommand(connection, stream);
                }
                case Command.ModifyTeam:
                {
                    return new LogicModifyTeamCommand(connection, stream);
                }
                case Command.BuyResource:
                {
                    return new LogicBuyResourceCommand(connection, stream);
                }
                case Command.StartMatchmake:
                {
                    return new LogicStartMatchmakeCommand(connection, stream);
                }
                case Command.KickAllianceMember:
                {
                    return new LogicKickAllianceMemberCommand(connection, stream);
                }
                case Command.BuyExtra:
                {
                    return new LogicBuyExtraCommand(connection, stream);
                }
                case Command.BuyBooster:
                {
                    return new LogicBuyBoosterCommand(connection, stream);
                }
                case Command.ClaimAchievement:
                {
                    return new LogicClaimAchievementCommand(connection, stream);
                }
                case Command.CollectMapChest:
                {
                    return new LogicCollectMapChestCommand(connection, stream);
                }
                case Command.StartSailing:
                {
                    return new LogicStartSailingCommand(connection, stream);
                }
                case Command.CollectShipReward:
                {
                    return new LogicCollectShipRewardCommand(connection, stream);
                }
                case Command.SpeedUpShip:
                {
                    return new LogicSpeedUpShipCommand(connection, stream);
                }
                case Command.FinishHeroUpgrade:
                {
                    return new LogicFinishHeroUpgradeCommand(connection, stream);
                }
                case Command.BuyEnergyPackage:
                {
                    return new LogicBuyEnergyPackageCommand(connection, stream);
                }
                case Command.UpgradeShip:
                {
                    return new LogicUpgradeShipCommand(connection, stream);
                }
                case Command.FinishShipUpgrade:
                {
                    return new LogicFinishShipUpgradeCommand(connection, stream);
                }
                case Command.UnlockSpellSlot:
                {
                    return new LogicUnlockSpellSlotCommand(connection, stream);
                }
                case Command.UnlockSpell:
                {
                    return new LogicUnlockSpellCommand(connection, stream);
                }
                case Command.BrewSpell:
                {
                    return new LogicBrewSpellCommand(connection, stream);
                }
                case Command.StopSpell:
                {
                    return new LogicStopSpellCommand(connection, stream);
                }
                case Command.FinishSpells:
                {
                    return new LogicFinishSpellsCommand(connection, stream);
                }
                case Command.BuyItem:
                {
                    return new LogicBuyItemCommand(connection, stream);
                }
                case Command.UpgradeItem:
                {
                    return new LogicUpgradeItemCommand(connection, stream);
                }
                case Command.AttachItem:
                {
                    return new LogicAttachItemCommand(connection, stream);
                }
                case Command.SpeedUpItem:
                {
                    return new LogicSpeedUpItemCommand(connection, stream);
                }
                case Command.MoveCharacter:
                {
                    return new LogicMoveCharacterCommand(connection, stream);
                }
                case Command.ReturnToMap:
                {
                    return new LogicReturnToMapCommand(connection, stream);
                }
                case Command.TurnTimedOut:
                {
                    return new LogicMultiplayerTurnTimedOutCommand(connection, stream);
                }
                case Command.SwapCharacter:
                {
                    return new LogicSwapCharacterCommand(connection, stream);
                }
                case Command.Resign:
                {
                    return new LogicResignCommand(connection, stream);
                }
                case Command.SendTaunt:
                {
                    return new LogicSendTauntCommand(connection, stream);
                }
                case Command.Unknown_1:
                {
                    return new LogicUnknownCommand(connection, stream);
                }
                case Command.UseSpell:
                {
                    return new LogicUseSpellCommand(connection, stream);
                }
                case Command.ActionSeen:
                {
                    return new LogicActionSeenCommand(connection, stream);
                }
                default:
                {
                    Debugger.Warning($"Command {type} does not exist.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Creates the chat command.
        /// </summary>
        internal static LogicChatCommand CreateChatCommand(string type, Connection connection, string[] arguments)
        {
            if (LogicCommandManager.ChatCommands.TryGetValue(type, out Type cType))
            {
                return (LogicChatCommand)Activator.CreateInstance(cType, connection, arguments);
            }

            connection.SendChatMessage(LogicCommandManager.PrintHelp(type));

            return null;
        }

        /// <summary>
        /// Determines whether the specified <see cref="LogicCommand"/> is allowed in the current state.
        /// </summary>
        internal static bool IsCommandAllowedInCurrentState(LogicCommand command)
        {
            Connection connection = command.Connection;
            int type              = (int)command.Type;

            if (connection.IsConnected)
            {
                if (LogicVersion.IsProd || connection.Avatar.Rank != Rank.Administrator)
                {
                    if (type >= 1000)
                    {
                        Debugger.Error("Execute command failed! Debug commands are not allowed when debug is off.");
                        return false;
                    }
                }

                if (type >= 600 && type < 700)
                {
                    if (connection.State != State.Battle)
                    {
                        Debugger.Error($"Execute command failed! Command {type} is only allowed in battle state. Avatar {connection.Avatar}");
                        return false;
                    }
                }
                if (type >= 500 && type <= 598)
                {
                    if (connection.State != State.Home)
                    {
                        Debugger.Error($"Execute command failed! Command {type} is only allowed in home state. Avatar {connection.Avatar}");
                        return false;
                    }
                }
            }

            return true;
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
            foreach (string command in LogicCommandManager.ChatCommands.Keys)
            {
                help.AppendLine($"\t /{command}");
            }

            return help.ToString();
        }
    }
}
