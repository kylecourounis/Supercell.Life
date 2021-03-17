namespace Supercell.Life.Server.Files.CsvHelpers
{
    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;

    internal class LogicDataTable
    {
        internal Table Table;
        internal int Index;

        internal LogicArrayList<LogicData> Datas;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataTable"/> class.
        /// </summary>
        internal LogicDataTable()
        {
            this.Datas = new LogicArrayList<LogicData>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataTable"/> class.
        /// </summary>
        public LogicDataTable(Table table, int index)
        {
            this.Table = table;
            this.Index = index;

            this.Datas = new LogicArrayList<LogicData>();

            for (int i = 0; i < table.GetRowCount(); i++)
            {
                Row row        = table.GetRowAt(i);
                LogicData data = this.Create(row);

                this.Datas.Add(data);
            }
        }

        /// <summary>
        /// Creates the data for the specified row.
        /// </summary>
        internal LogicData Create(Row row)
        {
            LogicData data;

            switch (this.Index)
            {
                case 1:
                {
                    data = new LogicLocaleData(row, this);
                    break;
                }

                case 2:
                {
                    data = new LogicResourceData(row, this);
                    break;
                }

                case 3:
                {
                    data = new LogicEffectData(row, this);
                    break;
                }

                case 4:
                {
                    data = new LogicParticleEmitterData(row, this);
                    break;
                }

                case 5:
                {
                    data = new LogicGlobalData(row, this);
                    break;
                }

                case 6:
                {
                    data = new LogicQuestData(row, this);
                    break;
                }

                case 8:
                {
                    data = new LogicAchievementData(row, this);
                    break;
                }

                case 10:
                {
                    data = new LogicWorldData(row, this);
                    break;
                }

                case 11:
                {
                    data = new LogicHeroData(row, this);
                    break;
                }

                case 12:
                {
                    data = new LogicExperienceLevelData(row, this);
                    break;
                }

                case 13:
                {
                    data = new LogicLeagueData(row, this);
                    break;
                }

                case 14:
                {
                    data = new LogicObstacleData(row, this);
                    break;
                }

                case 21:
                {
                    data = new LogicAllianceBadgeData(row, this);
                    break;
                }

                case 24:
                {
                    data = new LogicTauntData(row, this);
                    break;
                }

                case 25:
                {
                    data = new LogicDecoData(row, this);
                    break;
                }

                case 26:
                {
                    data = new LogicVariableData(row, this);
                    break;
                }
               
                case 28:
                {
                    data = new LogicBoosterData(row, this);
                    break;
                }

                case 32:
                {
                    data = new LogicEnergyPackageData(row, this);
                    break;
                }

                case 33:
                {
                    data = new LogicTeamGoalData(row, this);
                    break;
                }

                case 35:
                {
                    data = new LogicSpellData(row, this);
                    break;
                }

                case 36:
                {
                    data = new LogicEventsData(row, this);
                    break;
                }

                case 37:
                {
                    data = new LogicItemsData(row, this);
                    break;
                }

                default:
                {
                    data = new LogicData(row, this);
                    break;
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the data with identifier.
        /// </summary>
        internal LogicData GetDataWithID(int id)
        {
            return this.Datas[GlobalID.GetID(id)];
        }

        /// <summary>
        /// Gets the data with instance identifier.
        /// </summary>
        internal LogicData GetDataWithInstanceID(int id)
        {
            return this.Datas[id];
        }

        /// <summary>
        /// Gets the data using its name.
        /// </summary>
        internal LogicData GetDataByName(string name)
        {
            return this.Datas.Find(data => data.Row.Name.Equals(name));
        }
    }
}