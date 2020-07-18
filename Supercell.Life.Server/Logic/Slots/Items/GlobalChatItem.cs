namespace Supercell.Life.Server.Logic.Slots.Items
{
    using Supercell.Life.Server.Logic.Avatar;

    internal struct GlobalChatItem
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
        /// Gets or sets a value indicating whether this <see cref="GlobalChatItem"/> will be sent from the system.
        /// </summary>
        internal bool System
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GlobalChatItem"/> will use a regex.
        /// </summary>
        internal bool Regex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets an empty instance of <see cref="GlobalChatItem"/>.
        /// </summary>
        internal static GlobalChatItem Empty
        {
            get
            {
                return new GlobalChatItem
                {
                    Message = string.Empty
                };
            }
        }
    }
}
