namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicActionSeenCommand : LogicCommand
    {
        internal int SeenType;
        internal int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicActionSeenCommand"/> class.
        /// </summary>
        public LogicActionSeenCommand(Connection connection) : base(connection)
        {
            // LogicActionSeenCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            this.SeenType = stream.ReadInt();
            this.Value    = stream.ReadInt();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (this.Value > 0)
            {
                switch (this.SeenType)
                {
                    case 0:
                    {
                        if (CSV.Tables.GetWithGlobalID(this.Value) is LogicQuestData quest)
                        {
                            gamemode.Avatar.QuestUnlockSeens.Set(quest.GlobalID, 1);
                        }
                        else
                        {
                            Debugger.Error("QuestData is null or not valid.");
                        }

                        break;
                    }
                    case 1:
                    {
                        if (CSV.Tables.GetWithGlobalID(this.Value) is LogicHeroData hero)
                        {
                            gamemode.Avatar.HeroUnlockSeens.Set(hero.GlobalID, 1);
                        }
                        else
                        {
                            Debugger.Error("HeroData is null or not valid.");
                        }

                        break;
                    }
                    case 2:
                    {
                        gamemode.Avatar.TutorialMask |= this.Value;
                        break;
                    }
                    case 3:
                    {
                        gamemode.Avatar.Variables.Set(LogicVariables.IntroSeen.GlobalID, 1);
                        break;
                    }
                    default:
                    {
                        Debugger.Warning($"{this.SeenType} : {this.Value}");
                        break;
                    }
                }
            }   
        }
    }
}