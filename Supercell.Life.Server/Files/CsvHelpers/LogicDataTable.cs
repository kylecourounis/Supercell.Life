namespace Supercell.Life.Server.Files.CsvHelpers
{
    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicDataTable
    {
        internal Table Table;
        internal int Index;

        internal LogicArrayList<LogicData> Datas;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataTable"/> class.
        /// </summary>
        internal LogicDataTable(Table table, int index)
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

            switch ((Gamefile)this.Index)
            {
                case Gamefile.Locales:
                {
                    data = new LogicLocaleData(row, this);
                    break;
                }

                case Gamefile.Resources:
                {
                    data = new LogicResourceData(row, this);
                    break;
                }

                case Gamefile.Effects:
                {
                    data = new LogicEffectData(row, this);
                    break;
                }

                case Gamefile.ParticleEmitters:
                {
                    data = new LogicParticleEmitterData(row, this);
                    break;
                }

                case Gamefile.Globals:
                {
                    data = new LogicGlobalData(row, this);
                    break;
                }

                case Gamefile.Quests:
                {
                    data = new LogicQuestData(row, this);
                    break;
                }

                case Gamefile.Achievements:
                {
                    data = new LogicAchievementData(row, this);
                    break;
                }

                case Gamefile.Worlds:
                {
                    data = new LogicWorldData(row, this);
                    break;
                }

                case Gamefile.Heroes:
                {
                    data = new LogicHeroData(row, this);
                    break;
                }

                case Gamefile.EnemyCharacters:
                {
                    data = new LogicEnemyCharacterData(row, this);
                    break;
                }

                case Gamefile.Obstacles:
                {
                    data = new LogicObstacleData(row, this);
                    break;
                }

                case Gamefile.Collectables:
                {
                    data = new LogicCollectableData(row, this);
                    break;
                }

                case Gamefile.ExperienceLevels:
                {
                    data = new LogicExperienceLevelData(row, this);
                    break;
                }

                case Gamefile.Leagues:
                {
                    data = new LogicLeagueData(row, this);
                    break;
                }

                case Gamefile.AllianceBadges:
                {
                    data = new LogicAllianceBadgeData(row, this);
                    break;
                }

                case Gamefile.Dialog:
                {
                    data = new LogicDialogData(row, this);
                    break;
                }

                case Gamefile.Taunts:
                {
                    data = new LogicTauntData(row, this);
                    break;
                }

                case Gamefile.Decos:
                {
                    data = new LogicDecoData(row, this);
                    break;
                }

                case Gamefile.Variables:
                {
                    data = new LogicVariableData(row, this);
                    break;
                }
               
                case Gamefile.Boosters:
                {
                    data = new LogicBoosterData(row, this);
                    break;
                }

                case Gamefile.EnergyPackages:
                {
                    data = new LogicEnergyPackageData(row, this);
                    break;
                }

                case Gamefile.TeamGoals:
                {
                    data = new LogicTeamGoalData(row, this);
                    break;
                }

                case Gamefile.Spells:
                {
                    data = new LogicSpellData(row, this);
                    break;
                }

                case Gamefile.Events:
                {
                    data = new LogicEventsData(row, this);
                    break;
                }

                case Gamefile.Items:
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