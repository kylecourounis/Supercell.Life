namespace Supercell.Life.Server.Protocol.Commands
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Client;

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
            var command = LogicCommandManager.CreateCommand(id, this.Connection, stream);

            if (command != null)
            {
                if (this.IsCommandAllowedInCurrentState(command))
                {
                    Debugger.Info($"Command {command.GetType().Name.Pad(34)} received from {this.Connection.EndPoint}.");

                    command.Subtick = subtick;
                    command.Decode();

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
            var command = LogicCommandManager.CreateCommand(id, this.Connection, stream);

            if (id >= 600)
            {
                if (command != null)
                {
                    if (this.IsCommandAllowedInCurrentState(command))
                    {
                        Debugger.Info($"Battle Command {command.GetType().Name.Pad(34)} received from {this.Connection.EndPoint}.");

                        command.Subtick = subtick;
                        command.Decode();

                        this.SectorCommands.Add(command);
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
                    if (this.Connection.Avatar.Time.ClientSubTick <= command.ExecuteSubTick)
                    {
                        this.Connection.Avatar.Time.ClientSubTick = command.ExecuteSubTick;
                        this.Connection.Avatar.Tick();

                        command.Execute();
                    }
                }
                else Debugger.Error($"Execute command failed! Command should already executed. (type={command.Type}, server_tick)");
            }
            else Debugger.Error("Execute command failed! subtick is not valid.");
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
                case Command.AimCharacter:
                {
                    return new LogicAimCharacterCommand(connection, stream);
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
    }
}
