namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Items;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicBuyExtraCommand : LogicCommand
    {
        internal int DataType;
        internal LogicData Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyExtraCommand"/> class.
        /// </summary>
        public LogicBuyExtraCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicBuyExtraCommand.
        }

        internal override void Decode()
        {
            this.DataType = this.Stream.ReadInt();
            this.Data     = this.Stream.ReadDataReference();

            this.Stream.ReadInt();

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (this.Data != null)
            {
                if (this.DataType == 0)
                {
                    LogicTauntData taunt = (LogicTauntData) this.Data;

                    if (!gamemode.Avatar.Extras.ContainsKey(taunt.GlobalID))
                    {
                        if (!string.IsNullOrEmpty(taunt.CharacterUnlock))
                        {
                            LogicHeroData hero = (LogicHeroData) CSV.Tables.Get(Gamefile.Heroes).GetDataByName(taunt.CharacterUnlock);

                            if (!gamemode.Avatar.HeroLevels.ContainsKey(hero.GlobalID))
                            {
                                Debugger.Error($"Unable to buy the taunt. {gamemode.Avatar.Name} ({gamemode.Avatar}) doesn't have the hero {taunt.CharacterUnlock}.");
                                return;
                            }
                        }

                        if (taunt.Cost > 0)
                        {
                            LogicResourceData resource = (LogicResourceData) CSV.Tables.Get(Gamefile.Resources).GetDataByName(taunt.Resource);

                            if (gamemode.Avatar.Resources.GetCount(resource.GlobalID) < taunt.Cost)
                            {
                                Debugger.Error($"Unable to buy the taunt. {gamemode.Avatar.Name} does not have enough {taunt.Resource}. {taunt.Resource} : {gamemode.Avatar.Resources.GetCount(resource.GlobalID)}, Require : {taunt.Cost}.");
                                return;
                            }

                            gamemode.Avatar.Resources.Remove(resource.GlobalID, taunt.Cost);
                        }

                        gamemode.Avatar.Extras.Set(taunt.GlobalID, 1);
                    }
                }
                else
                {
                    LogicDecoData deco = (LogicDecoData) this.Data;

                    if (!gamemode.Avatar.Extras.ContainsKey(deco.GlobalID))
                    {
                        if (deco.UnlockLevel > gamemode.Avatar.ExpLevel)
                        {
                            Debugger.Error($"Unable to buy the deco. {gamemode.Avatar.Name} ({gamemode.Avatar}) is not at the required level.");
                            return;
                        }

                        if (deco.Cost > 0)
                        {
                            LogicResourceData resource = (LogicResourceData) CSV.Tables.Get(Gamefile.Resources).GetDataByName(deco.Resource);

                            if (gamemode.Avatar.Resources.GetCount(resource.GlobalID) < deco.Cost)
                            {
                                Debugger.Error($"Unable to buy the deco. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough {deco.Resource}. ({deco.Resource} : {gamemode.Avatar.Resources.GetCount(resource.GlobalID)}, Require : {deco.Cost}.");
                                return;
                            }

                            gamemode.Avatar.Resources.Remove(resource.GlobalID, deco.Cost);
                        }

                        gamemode.Avatar.Extras.Set(deco.GlobalID, 2);

                        if (gamemode.Avatar.Extras.Count > 1)
                        {
                            foreach (Item item in gamemode.Avatar.Extras.Values)
                            {
                                if (item.Id / 1000000 == deco.GetDataType())
                                {
                                    if (item.Count != 1 && item.Id != deco.GlobalID)
                                    {
                                        item.Count = 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (gamemode.Avatar.Extras.GetCount(deco.GlobalID) == 1)
                        {
                            gamemode.Avatar.Extras.Set(deco.GlobalID, 2);

                            foreach (Item item in gamemode.Avatar.Extras.Values)
                            {
                                if (item.Id / 1000000 == deco.GetDataType())
                                {
                                    if (item.Count != 1 && item.Id != deco.GlobalID)
                                    {
                                        item.Count = 1;
                                    }
                                }
                            }
                        }
                        else gamemode.Avatar.Extras.Set(deco.GlobalID, 1);
                    }
                }
            }
            else Debugger.Error("Unable to buy the extra. Data is null.");
        }
    }
}