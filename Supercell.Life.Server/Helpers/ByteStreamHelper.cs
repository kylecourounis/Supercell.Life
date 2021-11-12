namespace Supercell.Life.Server.Helpers
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;

    internal static class ByteStreamHelper
    {
        /// <summary>
        /// Reads a data reference.
        /// </summary>
        internal static LogicData ReadDataReference(this ByteStream stream, bool debug = false)
        {
            return stream.ReadDataReference<LogicData>(debug);
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        internal static T ReadDataReference<T>(this ByteStream stream, bool debug = false) where T : LogicData
        {
            int globalId = stream.ReadInt();

            if (debug)
            {
                Debugger.Debug($"{typeof(T).Name} - {globalId}");
            }

            return CSV.Tables.GetWithGlobalID(globalId) as T;
        }

        /// <summary>
        /// Writes a data reference.
        /// </summary>
        internal static void WriteDataReference(this ChecksumEncoder encoder, LogicData data)
        {
            encoder.WriteInt(data?.GlobalID ?? 0);
        }
    }
}
