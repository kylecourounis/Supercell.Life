namespace Supercell.Life.Server.Files
{
    using System;
    using System.IO;

    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic.Json;

    public static class Fingerprint
    {
        public static string Json;
        public static string Sha;
        public static string[] Version;

        public static bool Custom;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Fingerprint"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Fingerprint"/> class.
        /// </summary>
        public static void Init()
        {
            if (Fingerprint.Initialized)
            {
                return;
            }

            try
            {
                if (!Fingerprint.Patched)
                {
                    FileInfo file = new FileInfo($@"Gamefiles\fingerprint.json");

                    if (file.Exists)
                    {
                        Fingerprint.Json     = file.ReadAllText();

                        LogicJSONObject json = LogicJSONParser.ParseObject(Fingerprint.Json);
                        Fingerprint.Sha      = json.GetJsonString("sha").GetStringValue();
                        Fingerprint.Version  = json.GetJsonString("version").GetStringValue().Split('.');
                    }
                    else
                    {
                        Debugger.Error("The Fingerprint cannot be loaded, the file does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                Debugger.Error($"{exception.GetType().Name} while parsing the fingerprint.");
            }

            Fingerprint.Initialized = true;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Fingerprint"/> is patched.
        /// </summary>
        private static bool Patched
        {
            get
            {
                FileInfo version = new FileInfo($@"{Directory.GetCurrentDirectory()}\Patch\VERSION");

                if (version.Exists)
                {
                    string[] lines = version.ReadAllLines();

                    if (!string.IsNullOrEmpty(lines[0]))
                    {
                        Fingerprint.Version = lines[0].Split('.');

                        if (lines.Length > 1 && !string.IsNullOrEmpty(lines[1]))
                        {
                            Fingerprint.Sha = lines[1];
                            
                            FileInfo patchedFingerprint = new FileInfo($@"{Directory.GetCurrentDirectory()}\Patchs\{Fingerprint.Sha}\fingerprint.json");

                            if (patchedFingerprint.Exists)
                            {
                                Fingerprint.Json = patchedFingerprint.ReadAllText().Trim('\n', '\r');
                                
                                Fingerprint.Custom = true;
                            }
                        }
                    }
                }

                return Fingerprint.Custom;
            }
        }
    }
}