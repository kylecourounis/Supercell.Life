namespace Supercell.Life.Titan.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    public static class Natives
    {
        /// <summary>
        /// Sets up the console.
        /// </summary>
        public static void SetupConsole()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                var window = Natives.GetConsoleWindow();

                Natives.SetWindowLong(window, -20, (int)Natives.GetWindowLong(window, -20) ^ 0x80000);
                Natives.SetLayeredWindowAttributes(window, 0, 227, 0x2);
            }
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int newLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr window, uint key, byte alpha, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowLong(IntPtr window, int index);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr window, int showMode);
    }
}
