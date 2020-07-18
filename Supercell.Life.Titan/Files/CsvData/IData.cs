namespace Supercell.Life.Titan.Files.CsvData
{
    public interface IData
    {
        /// <summary>
        /// Gets the global identifier.
        /// </summary>
        int GlobalId
        {
            get;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name
        {
            get;
            set;
        }
    }
}