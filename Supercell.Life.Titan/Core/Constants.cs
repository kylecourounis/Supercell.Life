namespace Supercell.Life.Titan.Core
{
    using System.Net;
    using System.Net.Sockets;

    public static class Constants
    {
        /// <summary>
        /// If set to true, the server will only accept authorized ip's.
        /// </summary>
        public const bool Local     = false;

        /// <summary>
        /// The length of the buffer used to send/receive packets.
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// The maximum of players the server can handle at one time.
        /// </summary>
        public const int MaxPlayers = 100;

        /// <summary>
        /// The maximum amount of send operations the server can process.
        /// </summary>
        public const int MaxSends   = Constants.MaxPlayers * 10;

        /// <summary>
        /// Returns this computer's public IP address.
        /// </summary>
        public static IPAddress PublicIP
        {
            get
            {
                return IPAddress.Parse(new WebClient().DownloadString("http://api.ipify.org/"));
            }
        }

        /// <summary>
        /// Returns this computer's local IP address.
        /// </summary>
        public static IPAddress LocalIP
        {
            get
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
                    {
                        socket.Connect("10.0.2.4", 65530);
                        return ((IPEndPoint)socket.LocalEndPoint).Address;
                    }
                }
                catch
                {
                    return IPAddress.Parse("127.0.0.1");
                }
            }
        }
    }
}