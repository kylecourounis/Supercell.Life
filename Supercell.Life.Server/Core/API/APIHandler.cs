namespace Supercell.Life.Server.Core.API
{
    using System;
    using System.Net;
    using System.Threading;

    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal static class APIHandler
    {
        private static HttpListener Listener;

        /// <summary>
        /// Gets a value indicating whether this <see cref="APIHandler"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="APIHandler"/> class.
        /// </summary>
        internal static void Init()
        {
            if (APIHandler.Initialized)
            {
                return;
            }

            try
            {
                APIHandler.Listener = new HttpListener();
                APIHandler.Listener.Prefixes.Add("http://127.0.0.1:8080/");
                APIHandler.Listener.Prefixes.Add("http://*:8080/");
                APIHandler.Listener.Start();

                APIHandler.Initialized = true;

                Console.WriteLine("API Server is listening on 0.0.0.0:8080.");

                ThreadPool.QueueUserWorkItem(async _ =>
                {
                    while (APIHandler.Listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(context =>
                        {
                            try
                            {
                                ((HttpListenerContext)context).EndProcess();
                            }
                            catch
                            {
                                return;
                            }
                        }, await APIHandler.Listener.GetContextAsync());
                    }
                });
            }
            catch
            {
                // Do nothing. There's no harm if it doesn't start.
            }
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        private static void EndProcess(this HttpListenerContext context)
        {
            LogicLong player = new LogicLong(0, LogicStringUtil.ConvertToInt(context.Request.QueryString["player"]));
            int debugAction  = LogicStringUtil.ConvertToInt(context.Request.QueryString["action"]);
            int argument     = LogicStringUtil.ConvertToInt(context.Request.QueryString["argument"]);
            int globalID     = LogicStringUtil.ConvertToInt(context.Request.QueryString["globalID"]);

            Connection connection = Avatars.Get(player).Connection;

            if (connection != null)
            {
                if (connection.IsConnected && LogicVersion.IsIntegration)
                {
                    new AvailableServerCommandMessage(connection, new LogicDebugCommand(connection, debugAction)
                    {
                        IntArgument = argument,
                        GlobalID = globalID
                    }).Send();

                    context.Response.Close(LogicStringUtil.GetBytes("OK"), true);
                }
                else
                {
                    context.Response.Close(LogicStringUtil.GetBytes("Error: Connection.IsConnected == false or the server is not in integration mode"), true);
                }
            }
            else
            {
                context.Response.Close(LogicStringUtil.GetBytes("Error: Connection == null"), true);
            }
        }
    }
}