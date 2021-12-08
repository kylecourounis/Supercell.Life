namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicSendFriendlyChallengeCommand : LogicCommand
    {
        internal string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSendFriendlyChallengeCommand"/> class.
        /// </summary>
        public LogicSendFriendlyChallengeCommand(Connection connection) : base(connection)
        {
            // LogicSendFriendlyChallengeCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            if (stream.ReadBoolean())
            {
                this.Message = stream.ReadString();
            }

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            // TODO
        }
    }
}
