namespace Supercell.Life.Server.Files.CsvLogic
{ 
    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;
   
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicHeroData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicHeroData"/> class.
        /// </summary>
        public LogicHeroData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        internal string Name
        {
            get
            {
                switch (this.TID)
                {
                    case "TID_ADVBOY":
                    {
                        return "AdvBoy";
                    }
                    case "TID_WIZARD":
                    {
                        return "Wizard";
                    }
                    case "TID_PRINCESS":
                    {
                        return "Princess";
                    }
                    case "TID_PIRATE":
                    {
                        return "Pirate";
                    }
                    case "TID_MUMMY":
                    {
                        return "Mummy";
                    }
                    case "TID_FAIRY":
                    {
                        return "Fairy";
                    }
                    case "TID_BARREL":
                    {
                        return "Barrel";
                    }
                    case "TID_SPACEG":
                    {
                        return "SpaceGirl";
                    }
                    case "TID_GENIE":
                    {
                        return "Genie";
                    }
                    case "TID_YETI":
                    {
                        return "Yeti";
                    }
                }

                return string.Empty;
            }
        }

        public string TID
        {
            get; set;
        }

        public string InfoTID
        {
            get; set;
        }

        public bool HiddenFromGame
        {
            get; set;
        }

        public string SWF
        {
            get; set;
        }

        public string ExportName
        {
            get; set;
        }

        public string ClipReplaceOld
        {
            get; set;
        }

        public string ClipReplaceNew
        {
            get; set;
        }

        public string SpecialIconSWF
        {
            get; set;
        }

        public int Scale
        {
            get; set;
        }

        public string SpecialIconExportName
        {
            get; set;
        }

        public string SpecialAttackAimClip
        {
            get; set;
        }

        public string SpecialAttackName
        {
            get; set;
        }

        public string SpecialAttackDescription
        {
            get; set;
        }

        public string PassiveAbilityName
        {
            get; set;
        }

        public string PassiveAbilityDescription
        {
            get; set;
        }

        public string PassiveAbilityIconSWF
        {
            get; set;
        }

        public string PassiveAbilityIconExportName
        {
            get; set;
        }

        public string IconSWF
        {
            get; set;
        }

        public string IconExportName
        {
            get; set;
        }

        public string IconSelectSWF
        {
            get; set;
        }

        public string IconSelectExportName
        {
            get; set;
        }

        public int SortingOrder
        {
            get; set;
        }

        public int RelativeStartingRank
        {
            get; set;
        }

        public LogicArrayList<int> UpgradeTimeSeconds
        {
            get; set;
        }

        public int RequiredXpLevel
        {
            get; set;
        }

        public string RequiredQuest
        {
            get; set;
        }

        public string ShowOnQuest
        {
            get; set;
        }

        public LogicArrayList<int> Damage
        {
            get; set;
        }

        public LogicArrayList<int> SpecialAttackDamage
        {
            get; set;
        }

        public LogicArrayList<int> Hitpoints
        {
            get; set;
        }

        public string Resource
        {
            get; set;
        }

        public LogicArrayList<string> Cost
        {
            get; set;
        }

        public int PushEnemyHeroMod
        {
            get; set;
        }

        public int SpecialAttackActiveLimit
        {
            get; set;
        }

        public int Speed
        {
            get; set;
        }

        public int Radius
        {
            get; set;
        }

        public string SpecialAttack
        {
            get; set;
        }

        public int SpecialAttackPushback
        {
            get; set;
        }

        public string SpecialAttackProjectile
        {
            get; set;
        }

        public string SpecialAttackSpawnObstacle
        {
            get; set;
        }

        public int SpecialAttackProjectileCount
        {
            get; set;
        }

        public int SpecialAttackKnockback
        {
            get; set;
        }

        public int SpecialAttackSpeedBoostDuration
        {
            get; set;
        }

        public string AttackEffect
        {
            get; set;
        }

        public string AttackEffect2
        {
            get; set;
        }

        public string FlickEffect
        {
            get; set;
        }

        public string MoveEffect
        {
            get; set;
        }

        public string SpecialAttackDragEffect
        {
            get; set;
        }

        public string SpecialFlickEffect
        {
            get; set;
        }

        public string SpecialHitEffect
        {
            get; set;
        }

        public string SpecialHitEffectObstacle
        {
            get; set;
        }

        public string SpecialHitEffectEdge
        {
            get; set;
        }

        public string SpecialMoveEffect
        {
            get; set;
        }

        public string EndSpecialEffect
        {
            get; set;
        }

        public string DrawBackSpecialLoop
        {
            get; set;
        }

        public string SpecialRollLoop
        {
            get; set;
        }

        public string TakeDamageEffect
        {
            get; set;
        }

        public string DieEffect
        {
            get; set;
        }

        public string RollSound
        {
            get; set;
        }

        public int ShadowHeight
        {
            get; set;
        }

        public string BecomesActiveEffect
        {
            get; set;
        }

        public string World
        {
            get; set;
        }

        public string BaseExportName
        {
            get; set;
        }

        public string PassiveAbility
        {
            get; set;
        }

        public int PassiveAbilityOpensRank
        {
            get; set;
        }

        public string TipPortraitSWF
        {
            get; set;
        }

        public string TipPortraitExportName
        {
            get; set;
        }

        public int TiredTimer
        {
            get; set;
        }

        internal LogicQuestData RequiredQuestData
        {
            get
            {
                return string.IsNullOrEmpty(this.RequiredQuest) ? null : (LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataByName(this.RequiredQuest);
            }
        }
    }
}
