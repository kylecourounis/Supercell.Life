namespace Supercell.Life.Titan.Helpers
{
    using System.IO;

    public static class IoExtensions
    {
        /// <summary>
        /// Creates a directory.
        /// </summary>
        public static FileInfo CreateIfNotExists(this FileInfo info)
        {
            if (!info.Exists)
            {
                info.Create().Close();
            }

            return info;
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        public static string ReadAllText(this FileInfo info)
        {
            return File.ReadAllText(info.FullName);
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        public static string[] ReadAllLines(this FileInfo info)
        {
            return File.ReadAllLines(info.FullName);
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        public static byte[] ReadAllBytes(this FileInfo info)
        {
            return File.ReadAllBytes(info.FullName);
        }
        
        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes the file. 
        /// If the target file already exists, it is overwritten.
        /// </summary>
        public static void WriteAllText(this FileInfo info, string text)
        {
            File.WriteAllText(info.FullName, text);
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file, and then closes the file. 
        /// </summary>
        public static void WriteAllLines(this FileInfo info, params string[] lines)
        {
            File.WriteAllLines(info.FullName, lines);
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. 
        /// If the target file already exists, it is overwritten.
        /// </summary>
        public static void WriteAllBytes(this FileInfo info, byte[] bytes)
        {
            File.WriteAllBytes(info.FullName, bytes);
        }

        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file.
        /// If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        public static void AppendAllText(this FileInfo info, string text)
        {
            File.AppendAllText(info.FullName, text);
        }

        /// <summary>
        /// Appends the lines to a file, and then closes the file.
        /// If the file does not exist, this method creates a file, writes the specified lines to the file, then closes the file.
        /// </summary>
        public static void AppendAllLines(this FileInfo info, params string[] lines)
        {
            File.AppendAllLines(info.FullName, lines);
        }

        /// <summary>
        /// Permanently deletes a file if it exists.
        /// </summary>
        public static void DeleteIfExists(this FileInfo info)
        {
            if (info.Exists)
            {
                info.Delete();
            }
        }

        /// <summary>
        /// Backs up a file to the specified location.
        /// </summary>
        public static void Backup(this FileInfo info, string location)
        {
            info.CopyTo(location);
        }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        public static void CreateIfNotExists(this DirectoryInfo info)
        {
            if (!info.Exists)
            {
                info.Create();
            }
        }

        /// <summary>
        /// Permanently deletes a file if it exists in the specified directory.
        /// </summary>
        public static void DeleteIfExists(this DirectoryInfo info, string name)
        {
            FileInfo[] files = info.GetFiles(name);

            if (files.Length > 0)
            {
                files[0].DeleteIfExists();
            }
        }

        /// <summary>
        /// Gets the number of files in the specified directory.
        /// </summary>
        public static int GetFileCount(this DirectoryInfo info)
        {
            return info.GetFiles().Length;
        }
    }
}
