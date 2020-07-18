namespace Supercell.Life.Titan.Core.Consoles
{
    using System;
    using System.IO;
    using System.Text;

    public class Prefixed : TextWriter
    {
        private readonly TextWriter Original;

        /// <summary>
        /// Initializes a new instance of the <see cref="Prefixed"/> class.
        /// </summary>
        public Prefixed()
        {
            this.Original = Console.Out;
        }

        /// <summary>
        /// When overridden in a derived class, returns the character encoding in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return new ASCIIEncoding();
            }
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        public override void WriteLine(string message)
        {
            if (message == null)
            {
                return;
            }

            this.Original.WriteLine($"[*] {message}");
        }

        /// <summary>
        /// Writes a line terminator to the text string or stream.
        /// </summary>
        public override void WriteLine()
        {
            this.Original.WriteLine();
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        public override void Write(string message)
        {
            this.Original.Write(message);
        }
    }
}