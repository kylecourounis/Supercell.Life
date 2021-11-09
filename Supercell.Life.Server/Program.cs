namespace Supercell.Life.Server
{
    using System;
    using System.Reflection;

    using Supercell.Life.Titan.Core.Consoles;
    using Supercell.Life.Titan.Helpers;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Core.Consoles;

    internal class Program
    {
        private const int Width  = 120;
        private const int Height = 32;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Natives.SetupConsole();

            Console.Title = $"{Assembly.GetExecutingAssembly().GetName().Name} | {DateTime.Now.Year} ©";

            Console.SetOut(new Prefixed());

            Console.SetWindowSize(Program.Width, Program.Height);
            Console.SetBufferSize(Program.Width, Program.Height);

            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.Write(@"
     _____                     _      
    / ____|                   | |     
   | (___  _ __ ___   __ _ ___| |__   
    \___ \| '_ ` _ \ / _` / __| '_ \ 
    ____) | | | | | | (_| \__ \ | | |
   |_____/|_| |_| |_|\__,_|___/_| |_|");

            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(@"
        _                     _ 
       | |                   | |
       | |     __ _ _ __   __| |
       | |    / _` | '_ \ / _` |
       | |___| (_| | | | | (_| |
       |______\__,_|_| |_|\__,_|      

            " + Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Starting..." + Environment.NewLine);

            Loader.Init();

            Console.WriteLine();

            Parser.Init();
        }
    }
}