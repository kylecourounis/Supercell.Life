﻿namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicUnlockSpellSlotCommand : LogicCommand
    {
        internal int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUnlockSpellSlotCommand"/> class.
        /// </summary>
        public LogicUnlockSpellSlotCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicUnlockSpellSlotCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.Variables.GetCount(LogicVariables.SpellSlotsUnlocked.GlobalID) < 3)
            {
                gamemode.Avatar.Variables.AddItem(LogicVariables.SpellSlotsUnlocked.GlobalID, 1);
            }
        }
    }
}