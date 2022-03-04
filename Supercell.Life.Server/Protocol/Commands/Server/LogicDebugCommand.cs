namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;
    using Supercell.Life.Titan.Logic.Enums;

    internal class LogicDebugCommand : LogicServerCommand
    {
        internal int DebugAction;

        internal int IntArgument;
        internal int GlobalID;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDebugCommand"/> class.
        /// </summary>
        public LogicDebugCommand(Connection connection, int debug) : base(connection)
        {
            this.Type        = Command.Debug;
            this.DebugAction = debug;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.DebugAction);
            encoder.WriteInt(this.IntArgument);
            encoder.WriteInt(this.GlobalID);

            base.Encode(encoder);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            bool sendOwnAvatarData = true;

            switch ((LogicDebugAction)this.DebugAction)
            {
                case LogicDebugAction.AddResources:
                {
                    gamemode.Avatar.Resources.AddItem(this.GlobalID, this.IntArgument);
                    Debugger.Info("Add resources");
                    break;
                }
                case LogicDebugAction.RemoveResources:
                {
                    gamemode.Avatar.Resources.Remove(this.GlobalID, this.IntArgument);
                    Debugger.Info("Remove resources");
                    break;
                }
                case LogicDebugAction.CompleteAllQuests:
                {
                    if (gamemode.Avatar.ExpLevel < 35)
                    {
                        gamemode.Avatar.ExpLevel = 35;
                        gamemode.Avatar.ExpPoints = 0;
                    }

                    foreach (var quest in gamemode.Avatar.Quests.Values)
                    {
                        gamemode.Avatar.NpcProgress.AddItem(quest.GlobalID, quest.Levels.Count);
                        gamemode.Avatar.QuestUnlockSeens.AddItem(quest.GlobalID, 1);
                    }

                    Debugger.Info("All quests completed");
                    break;
                }
                case LogicDebugAction.EnterLevel:
                {
                    LogicLevel level = gamemode.Avatar.Quests[this.GlobalID].Levels[this.IntArgument];

                    sendOwnAvatarData = false;

                    break;
                }
                case LogicDebugAction.RemoveAllEntites:
                {
                    if (this.Connection.State != State.Battle)
                    {
                        Debugger.Warning("Available in combat state only");
                    }

                    sendOwnAvatarData = false;

                    break;
                }
                case LogicDebugAction.IncreaseXPLevel:
                {
                    if (gamemode.Avatar.ExpLevel < 35)
                    {
                        gamemode.Avatar.ExpLevel++;
                        gamemode.Avatar.ExpPoints = 0;
    
                        Debugger.Info($"New XP Level {gamemode.Avatar.ExpLevel}");
                    }

                    break;
                }
                case LogicDebugAction.FastForward1Minute:
                {
                    gamemode.FastForward(60);
                    Debugger.Info("Fast forward 1 minute");
                    
                    break;
                }
                case LogicDebugAction.FastForward1Hour:
                {
                    gamemode.FastForward(3600);
                    Debugger.Info("Fast forward 1 hour");
                        
                    break;
                }
                case LogicDebugAction.FastForward1Day:
                {
                    gamemode.FastForward(86400);
                    Debugger.Info("Fast forward 24 hours");
                        
                    break;
                }
                case LogicDebugAction.FillEnergy:
                {
                    gamemode.Avatar.EnergyTimer.Stop();
                    gamemode.Avatar.Energy = gamemode.Avatar.MaxEnergy;

                    Debugger.Info("Energy filled");
                        
                    break;
                }
                case LogicDebugAction.Add100Trophies:
                case LogicDebugAction.Remove100Trophies:
                case LogicDebugAction.Add1000Trophies:
                case LogicDebugAction.Remove1000Trophies:
                {
                    int v44 = 100;

                    switch (this.DebugAction)
                    {
                        case 22:
                            v44 = -100;
                            break;
                        case 23:
                            v44 = 1000;
                            break;
                        case 24:
                            v44 = -1000;
                            break;
                    }
                        
                    Debugger.Info($"Change {gamemode.Avatar.Score} trophy score [new {gamemode.Avatar.Score + v44}]");

                    gamemode.Avatar.AddTrophyScoreHelper(v44);
                        
                    break;
                }
                case LogicDebugAction.Add100Diamonds:
                case LogicDebugAction.Remove100Diamonds:
                case LogicDebugAction.Add1000Diamonds:
                case LogicDebugAction.Remove1000Diamonds:
                {
                    int v35 = 100;
                    
                    switch (this.DebugAction)
                    {
                        case 26:
                            v35 = -100;
                            break;
                        case 36:
                            v35 = 1000;
                            break;
                        case 37:
                            v35 = -1000;
                            break;
                    }

                    Debugger.Info($"Change {gamemode.Avatar.Diamonds} diamond count [new {gamemode.Avatar.Diamonds + v35}]");

                    new AvailableServerCommandMessage(this.Connection, new LogicDiamondsAddedCommand(this.Connection)
                    {
                        Diamonds = v35
                    }).Send();

                    gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, v35);

                    sendOwnAvatarData = false;
                    
                    break;
                }
            }

            if (sendOwnAvatarData)
            {
                new OwnAvatarDataMessage(this.Connection).Send();
            }
        }

        internal enum LogicDebugAction
        {
            AddResources       = 0,
            RemoveResources    = 1,
            CompleteAllQuests  = 4,
            EnterLevel         = 7,
            RemoveAllEntites   = 9,
            IncreaseXPLevel    = 14,
            FastForward1Minute = 15,
            FastForward1Hour   = 16,
            FastForward1Day    = 17,
            FillEnergy         = 20,
            Add100Trophies     = 21,
            Remove100Trophies  = 22,
            Add1000Trophies    = 23,
            Remove1000Trophies = 24,
            Add100Diamonds     = 25,
            Remove100Diamonds  = 26,
            Add1000Diamonds    = 36,
            Remove1000Diamonds = 37
        }
    }
}
