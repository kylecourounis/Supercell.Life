namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Linq;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Titan.DataStream;

    internal class LogicAchievementProgress : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAchievementProgress"/> class.
        /// </summary>
        internal LogicAchievementProgress(LogicClientAvatar avatar) : base(avatar)
        {
            // LogicAchievementProgress.
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
        }

        /// <summary>
        /// Encodes the reward claimed data.
        /// </summary>
        internal void EncodeRewardClaimedData(ByteStream stream)
        {
            stream.WriteInt(this.Count);

            foreach (LogicDataSlot slot in this.Values)
            {
                stream.WriteInt(slot.Id);
                stream.WriteInt(1);
            }
        }

        /// <summary>
        /// Encodes the XP reward data.
        /// </summary>
        internal void EncodeXPRewardData(ByteStream stream)
        {
            stream.WriteInt(this.Count);

            foreach (LogicDataSlot slot in this.Values)
            {
                stream.WriteInt(slot.Id);
                stream.WriteInt(1);
            }
        }
    }
}