namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicAllianceRoleData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAllianceRoleData"/> class.
        /// </summary>
        public LogicAllianceRoleData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public int Level
        {
            get; set;
        }

        public string TID
        {
            get; set;
        }

        public bool CanInvite
        {
            get; set;
        }

        public bool CanSendMail
        {
            get; set;
        }

        public bool CanChangeAllianceSettings
        {
            get; set;
        }

        public bool CanAcceptJoinRequest
        {
            get; set;
        }

        public bool CanKick
        {
            get; set;
        }

        public bool CanBePromotedToLeader
        {
            get; set;
        }

        public bool CanPromoteToOwnLevel
        {
            get; set;
        }
    }
}
