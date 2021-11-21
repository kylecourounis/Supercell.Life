namespace Supercell.Life.Server.Logic.Game.Objects
{
    using Supercell.Life.Server.Logic.Avatar;

    internal class GlobalChatLine
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        internal string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        internal LogicClientAvatar Sender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating who sent the message.
        /// </summary>
        internal bool WhoSent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GlobalChatLine"/> will be sent from the system.
        /// </summary>
        internal bool System
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GlobalChatLine"/> will use a regex.
        /// </summary>
        internal bool Regex
        {
            get;
            set;
        }
    }
}
