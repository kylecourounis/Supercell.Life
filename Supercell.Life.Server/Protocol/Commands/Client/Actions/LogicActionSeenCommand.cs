namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicActionSeenCommand : LogicCommand
    {
        internal int SeenType;
        internal int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicActionSeenCommand"/> class.
        /// </summary>
        public LogicActionSeenCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicActionSeenCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();

            this.SeenType = this.Stream.ReadInt();
            this.Value    = this.Stream.ReadInt();
        }

        internal override void Execute()
        {
            if (this.Value > 0)
            {
                switch (this.SeenType)
                {
                    case 0:
                    {
                        if (CSV.Tables.GetWithGlobalID(this.Value) is LogicQuestData quest)
                        {
                            this.Connection.Avatar.QuestUnlockSeens.Set(quest.GlobalID, 1);
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
                            this.Connection.Avatar.HeroUnlockSeens.Set(hero.GlobalID, 1);
                        }
                        else
                        {
                            Debugger.Error("HeroData is null or not valid.");
                        }

                        break;
                    }
                    case 2:
                    {
                        this.Connection.Avatar.TutorialMask |= this.Value;
                        break;
                    }
                    case 3:
                    {
                        this.Connection.Avatar.Variables.Set(LogicVariables.IntroSeen.GlobalID, 1);
                        break;
                    }
                    default:
                    {
                        Debugger.Warning($"{this.SeenType} : {this.Value}");
                        break;
                    }
                }

                this.Connection.Avatar.Save();
            }   
        }
    }
}