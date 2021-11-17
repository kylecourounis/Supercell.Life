namespace Supercell.Life.Server.Protocol.Messages
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Client;

    internal static class LogicLifeMessageFactory
    {
        internal const string RC4Key = "asfddsfsd874397f7d98wq090qd80qw";

        /// <summary>
        /// Creates a <see cref="PiranhaMessage"/> using the specified identifier.
        /// </summary>
        internal static PiranhaMessage CreateMessageByType(int id, Connection connection, ByteStream stream)
        {
            switch ((Message)id)
            {
                case Message.Login:
                {
                    return new LoginMessage(connection, stream);
                }
                case Message.KeepAlive:
                {
                    return new KeepAliveMessage(connection, stream);
                }
                case Message.SetDeviceToken:
                {
                    return new SetDeviceTokenMessage(connection, stream);
                }
                case Message.ChangeName:
                {
                    return new ChangeNameMessage(connection, stream);
                }
                case Message.AskForFacebookFriends:
                {
                    return new AskForPlayingFacebookFriendsMessage(connection, stream);
                }
                case Message.SectorCommand:
                {
                    return new SectorCommandMessage(connection, stream);
                }
                case Message.Unknown_1:
                {
                    return new UnknownMessage(connection, stream);
                }
                case Message.SendBattleEvent:
                {
                    return new SendBattleEventMessage(connection, stream);
                }
                case Message.GoHome:
                {
                    return new GoHomeMessage(connection, stream);
                }
                case Message.EndClientTurn:
                {
                    return new EndClientTurnMessage(connection, stream);
                }
                case Message.HomeLogicStopped:
                {
                    return new HomeLogicStoppedMessage(connection, stream);
                }
                case Message.SectorEndClientTurn:
                {
                    return new SectorEndClientTurnMessage(connection, stream);
                }
                case Message.CancelMatchmake:
                {
                    return new CancelMatchmakeMessage(connection, stream);
                }
                case Message.BindFacebook:
                {
                    return new BindFacebookAccountMessage(connection, stream);
                }
                case Message.BindGamecenter:
                {
                    return new BindGamecenterAccountMessage(connection, stream);
                }
                case Message.CreateAlliance:
                {
                    return new CreateAllianceMessage(connection, stream);
                }
                case Message.AskForAllianceData:
                {
                    return new AskForAllianceDataMessage(connection, stream);
                }
                case Message.AskForJoinableAlliances:
                {
                    return new AskForJoinableAlliancesListMessage(connection, stream);
                }
                case Message.JoinAlliance:
                {
                    return new JoinAllianceMessage(connection, stream);
                }
                case Message.ChangeAllianceMemberRole:
                {
                    return new ChangeAllianceMemberRoleMessage(connection, stream);
                }
                case Message.LeaveAlliance:
                {
                    return new LeaveAllianceMessage(connection, stream);
                }
                case Message.ChatToAlliance:
                {
                    return new ChatToAllianceStreamMessage(connection, stream);
                }
                case Message.ChangeAllianceSettings:
                {
                    return new ChangeAllianceSettingsMessage(connection, stream);
                }
                case Message.AllianceInvitation:
                {
                    return new SendAllianceInvitationMessage(connection, stream);
                }
                case Message.AskForAvatarProfile:
                {
                    return new AskForAvatarProfileMessage(connection, stream);
                }
                case Message.AskForAllianceRankingList:
                {
                    return new AskForAllianceRankingListMessage(connection, stream);
                }
                case Message.AskForAvatarRankingList:
                {
                    return new AskForAvatarRankingListMessage(connection, stream);
                }
                case Message.AskForAvatarStream:
                {
                    return new AskForAvatarStreamMessage(connection, stream);
                }
                case Message.SendGlobalChatLine:
                {
                    return new SendGlobalChatLineMessage(connection, stream);
                }
                default:
                {
                    Debugger.Warning($"Failed to handle a message with an ID of {id}!");
                    return null;
                }
            }
        }

        /// <summary>
        /// Sends the specified <see cref="PiranhaMessage"/>.
        /// </summary>
        internal static void Send(this PiranhaMessage message) => message.Connection.Messaging.Send(message);
    }
}