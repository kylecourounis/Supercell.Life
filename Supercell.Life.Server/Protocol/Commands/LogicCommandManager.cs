namespace Supercell.Life.Server.Protocol.Commands
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Client;
    using Supercell.Life.Server.Protocol.Commands.Server;

    internal class LogicCommandManager
    {
        internal readonly Connection Connection;

        internal readonly LogicArrayList<LogicCommand> Commands;
        internal readonly LogicArrayList<LogicCommand> SectorCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCommandManager"/> class.
        /// </summary>
        internal LogicCommandManager(Connection connection)
        {
            this.Connection     = connection;
            this.Commands       = new LogicArrayList<LogicCommand>();
            this.SectorCommands = new LogicArrayList<LogicCommand>();
        }
        
        /// <summary>
        /// Decodes a <see cref="LogicCommand"/>.
        /// </summary>
        internal void DecodeCommand(ByteStream stream, int subtick)
        {
            int id      = stream.ReadInt();
            var command = LogicCommandManager.CreateCommand(id, this.Connection);

            if (command != null)
            {
                if (this.IsCommandAllowedInCurrentState(command))
                {
                    Debugger.Info($"Command {command.GetType().Name.Pad(34)} received from {this.Connection.EndPoint}.");

                    command.Subtick = subtick;
                    command.Decode(stream);

                    this.Commands.Add(command);
                }
            }
            else
            {
                Debugger.Debug(stream.ToHexa());
            }
        }

        /// <summary>
        /// Decodes a <see cref="LogicCommand"/>.
        /// </summary>
        internal void DecodeBattleCommand(ByteStream stream, int subtick)
        {
            int id      = stream.ReadInt();
            var command = LogicCommandManager.CreateCommand(id, this.Connection);

            if (id >= 600)
            {
                if (command != null)
                {
                    // if (this.IsCommandAllowedInCurrentState(command))
                    {
                        Debugger.Info($"Battle Command {command.GetType().Name.Pad(34)} received from {this.Connection.EndPoint}.");

                        command.Subtick = subtick;
                        command.Decode(stream);
                        command.Execute(this.Connection.GameMode);

                        // this.SectorCommands.Add(command);
                    }
                }
                else
                {
                    Debugger.Debug(stream.ToHexa());
                }
            }
        }

        /// <summary>
        /// Executes the specified <see cref="LogicCommand"/>.
        /// </summary>
        internal void ExecuteCommand(LogicCommand command)
        {
            if (command.ExecuteSubTick != -1)
            {
                if (command.ExecuteSubTick <= command.Subtick)
                {
                    if (this.Connection.GameMode.Avatar.Time.ClientSubTick <= command.ExecuteSubTick)
                    {
                        this.Connection.GameMode.Avatar.Time.ClientSubTick = command.ExecuteSubTick;
                        this.Connection.GameMode.Tick();

                        command.Execute(this.Connection.GameMode);
                    }
                }
                else Debugger.Error($"Execute command failed! Command should have already executed. (type={command.Type}, server_tick)");
            }
            else Debugger.Error("Execute command failed! subtick is not valid.");
        }

        /// <summary>
        /// Loads the <see cref="LogicCommandManager"/> from the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal static LogicCommand LoadCommandFromJSON(LogicJSONObject json, Connection connection)
        {
            var id = json.GetJsonNumber("ct");

            if (id != null)
            {
                LogicCommand command = LogicCommandManager.CreateCommand(id.GetIntValue(), connection);

                command.LoadCommandFromJSON(json.GetJsonObject("c"));

                return command;
            }
            else
            {
                Debugger.Error("Unknown command type");
                return null;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="LogicCommand"/> is allowed in the current state.
        /// </summary>
        internal bool IsCommandAllowedInCurrentState(LogicCommand command)
        {
            Connection connection = command.Connection;
            int type = (int)command.Type;

            if (connection.IsConnected)
            {
                if (LogicVersion.IsProd || connection.GameMode.Avatar.Rank != Rank.Administrator)
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
                        Debugger.Error($"Execute command failed! Command {type} is only allowed in battle state. Avatar {connection.GameMode.Avatar}");
                        return false;
                    }
                }
                if (type >= 500 && type <= 598)
                {
                    if (connection.State != State.Home)
                    {
                        Debugger.Error($"Execute command failed! Command {type} is only allowed in home state. Avatar {connection.GameMode.Avatar}");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a <see cref="LogicCommand"/> using the specified type.
        /// </summary>
        internal static LogicCommand CreateCommand(int type, Connection connection)
        {
            switch ((Command)type)
            {
                case Command.JoinAlliance:
                {
                    return new LogicJoinAllianceCommand(connection);
                }
                case Command.ChangeAllianceRole:
                {
                    return new LogicChangeAllianceRoleCommand(connection);
                }
                case Command.StartQuest:
                {
                    return new LogicStartQuestCommand(connection);
                }
                case Command.BuyHero:
                {
                    return new LogicBuyHeroCommand(connection);
                }
                case Command.UpgradeHero:
                {
                    return new LogicUpgradeHeroCommand(connection);
                }
                case Command.ModifyTeam:
                {
                    return new LogicModifyTeamCommand(connection);
                }
                case Command.BuyResource:
                {
                    return new LogicBuyResourceCommand(connection);
                }
                case Command.StartMatchmake:
                {
                    return new LogicStartMatchmakeCommand(connection);
                }
                case Command.KickAllianceMember:
                {
                    return new LogicKickAllianceMemberCommand(connection);
                }
                case Command.BuyExtra:
                {
                    return new LogicBuyExtraCommand(connection);
                }
                case Command.BuyBooster:
                {
                    return new LogicBuyBoosterCommand(connection);
                }
                case Command.ClaimAchievement:
                {
                    return new LogicClaimAchievementCommand(connection);
                }
                case Command.CollectMapChest:
                {
                    return new LogicCollectMapChestCommand(connection);
                }
                case Command.StartSailing:
                {
                    return new LogicStartSailingCommand(connection);
                }
                case Command.CollectShipReward:
                {
                    return new LogicCollectShipRewardCommand(connection);
                }
                case Command.SpeedUpShip:
                {
                    return new LogicSpeedUpShipCommand(connection);
                }
                case Command.FinishHeroUpgrade:
                {
                    return new LogicFinishHeroUpgradeCommand(connection);
                }
                case Command.BuyEnergyPackage:
                {
                    return new LogicBuyEnergyPackageCommand(connection);
                }
                case Command.UpgradeShip:
                {
                    return new LogicUpgradeShipCommand(connection);
                }
                case Command.FinishShipUpgrade:
                {
                    return new LogicFinishShipUpgradeCommand(connection);
                }
                case Command.SendAllianceMail:
                {
                    return new LogicSendAllianceMailCommand(connection);
                }
                case Command.FriendlyChallenge:
                {
                    return new LogicSendFriendlyChallengeCommand(connection);
                }
                case Command.UnlockSpellSlot:
                {
                    return new LogicUnlockSpellSlotCommand(connection);
                }
                case Command.UnlockSpell:
                {
                    return new LogicUnlockSpellCommand(connection);
                }
                case Command.BrewSpell:
                {
                    return new LogicBrewSpellCommand(connection);
                }
                case Command.StopSpell:
                {
                    return new LogicStopSpellCommand(connection);
                }
                case Command.FinishSpells:
                {
                    return new LogicFinishSpellsCommand(connection);
                }
                case Command.BuyItem:
                {
                    return new LogicBuyItemCommand(connection);
                }
                case Command.UpgradeItem:
                {
                    return new LogicUpgradeItemCommand(connection);
                }
                case Command.AttachItem:
                {
                    return new LogicAttachItemCommand(connection);
                }
                case Command.SpeedUpItem:
                {
                    return new LogicSpeedUpItemCommand(connection);
                }
                case Command.MoveCharacter:
                {
                    return new LogicMoveCharacterCommand(connection);
                }
                case Command.ReturnToMap:
                {
                    return new LogicReturnToMapCommand(connection);
                }
                case Command.TurnTimedOut:
                {
                    return new LogicMultiplayerTurnTimedOutCommand(connection);
                }
                case Command.SwapCharacter:
                {
                    return new LogicSwapCharacterCommand(connection);
                }
                case Command.Resign:
                {
                    return new LogicResignCommand(connection);
                }
                case Command.SpeechBubbleReplay:
                {
                    return new LogicSpeechBubbleForReplayCommand(connection);
                }
                case Command.AimArrowForReplay:
                {
                    return new LogicAimingArrowForReplayCommand(connection);
                }
                case Command.UseSpell:
                {
                    return new LogicUseSpellCommand(connection);
                }
                case Command.ActionSeen:
                {
                    return new LogicActionSeenCommand(connection);
                }
                default:
                {
                    Debugger.Warning($"Command {type} does not exist.");
                    return null;
                }
            }
        }
    }
}
