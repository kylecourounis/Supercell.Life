namespace Supercell.Life.Server.Protocol.Commands.Debugs.Chat
{
    using System;

    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicRankCommand : LogicChatCommand
    {
        private string Tag;
        private Rank Rank;

        /// <summary>
        /// Gets the required rank.
        /// </summary>
        internal override Rank RequiredRank
        {
            get
            {
                return Rank.Administrator;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicRankCommand"/> class.
        /// </summary>
        public LogicRankCommand(Connection connection, params string[] parameters) : base(connection, parameters)
        {
            this.Help.AppendLine("Available Ranks:");
            this.Help.AppendLine("\t 0 = Normal Player");
            this.Help.AppendLine("\t 1 = Donator");
            this.Help.AppendLine("\t 2 = VIP");
            this.Help.AppendLine("\t 3 = YouTuber");
            this.Help.AppendLine("\t 4 = Moderator");
            this.Help.AppendLine("\t 5 = Administrator");

            this.Help.AppendLine("Command Usage:");
            this.Help.AppendLine("\t/rank {Tag} {Rank}");
        }

        internal override void Process()
        {
            if (this.Connection.Avatar.Rank >= this.RequiredRank)
            {
                if (this.Parameters.Length >= 2)
                {
                    try
                    {
                        if (Enum.TryParse(this.Parameters[1], out this.Rank))
                        {
                            if (this.Parameters[0] == this.Tag)
                            {
                                LogicLong id             = LogicTagUtil.ToLogicLong(this.Tag);
                                LogicClientAvatar avatar = Avatars.Get(id);
                                
                                if (this.Rank > this.Connection.Avatar.Rank)
                                {
                                    this.Connection.SendChatMessage("Target privileges are higher then yours.");
                                }
                                else
                                {
                                    if (avatar != null)
                                    {
                                        avatar.SetRank(this.Rank);
                                        this.Connection.SendChatMessage($"{avatar.Name} now has the rank {this.Rank}");
                                    }
                                    else
                                    {
                                        this.Help.AppendLine("Hashtags can only contain these characters:");
                                        this.Help.AppendLine("Numbers: 0, 2, 8, 9");
                                        this.Help.AppendLine("Letters: P, Y, L, Q, G, R, J, C, U, V");

                                        this.Connection.SendChatMessage(this.Help.ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.Connection.SendChatMessage(this.Help.ToString());
                        }
                    }
                    catch (Exception exception)
                    {
                        this.Connection.SendChatMessage($"Failed with error {exception.Message}");
                    }
                }
                else
                {
                    this.Connection.SendChatMessage(this.Help.ToString());
                }
            }
            else
            {
                this.Connection.SendChatMessage("Insufficient privileges.");
            }
        }
    }
}
