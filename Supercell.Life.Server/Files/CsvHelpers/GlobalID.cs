namespace Supercell.Life.Server.Files.CsvHelpers
{
    internal static class GlobalID
    {
        /// <summary>
        /// Gets the type from the specified global identifier.
        /// </summary>
        internal static int GetType(int globalID)
        {
            return globalID / 1000000;
        }

        /// <summary>
        /// Gets the identifier from the specified global identifier.
        /// </summary>
        internal static int GetID(int globalID)
        {
            return globalID - (GlobalID.GetType(globalID) * 1000000);
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        internal static int Create(int type, int id)
        {
            return type * 1000000 + id;
        }

        /// <summary>
        /// Creates the compressed data from the specified base type.
        /// </summary>
        internal static int CreateCompressed(int data, int baseType)
        {
            int id   = GlobalID.GetID(data) + 1;
            int type = GlobalID.GetType(data);

            if (type > baseType)
            {
                while (type > baseType)
                {
                    id += CSV.Tables.Get(baseType++).Datas.Size;
                }
            }

            return id;
        }

        /// <summary>
        /// Gets the compressed data.
        /// </summary>
        internal static int GetCompressed(int cid, int baseType)
        {
            int objectCount;

            cid--;

            while (baseType < 100)
            {
                if (cid >= (objectCount = CSV.Tables.Get(baseType).Datas.Size))
                {
                    baseType++;

                    cid -= objectCount;
                }
                else break;
            }

            return baseType * 1000000 + cid;
        }
    }
}