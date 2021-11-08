namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Linq;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicExtras : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicExtras"/> class.
        /// </summary>
        internal LogicExtras(LogicClientAvatar avatar) : base(avatar)
        {
            // LogicExtras.
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
            foreach (LogicTauntData taunt in CSV.Tables.Get(Gamefile.Taunts).Datas.Cast<LogicTauntData>().Where(taunt => taunt.UnlockedInBeginning))
            {
                this.AddItem(taunt.GlobalID, 2); // 1 = put in the inventory, 2 = equip
            }
        }
    }
}