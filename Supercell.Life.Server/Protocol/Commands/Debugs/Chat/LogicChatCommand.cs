namespace Supercell.Life.Server.Protocol.Commands
{
    using System.Text;

    using Supercell.Life.Titan.Logic.Enums;
    
    using Supercell.Life.Server.Network;

    internal class LogicChatCommand
    {
        /// <summary>
        /// Gets the Connection.
        /// </summary>
        internal Connection Connection
        {
            get;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        internal string[] Parameters
        {
            get;
        }

        /// <summary>
        /// Gets the required rank.
        /// </summary>
        internal virtual Rank RequiredRank
        {
            get
            {
                return Rank.Administrator;
            }
        }

        /// <summary>
        /// Gets the help.
        /// </summary>
        internal StringBuilder Help
        {
            get;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChatCommand"/> class.
        /// </summary>
        internal LogicChatCommand(Connection connection, params string[] parameters)
        {
            this.Connection = connection;
            this.Parameters = parameters;
            this.Help       = new StringBuilder();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal virtual void Process()
        {
            // Process.
        }
    }
}