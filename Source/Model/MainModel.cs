namespace RollingLineSavegameFix.Model
{
    public class MainModel
    {
        /// <summary>
        /// Path to Savegame File
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// Content of Savegame
        /// </summary>
        public string FileContent { get; set; }

        /// <summary>
        /// Removes faulty waggons on saving
        /// </summary>
        public bool ShouldRemoveFaultyWaggons { get; set; }

        /// <summary>
        /// Removes all Waggons on saving
        /// </summary>
        public bool ShouldRemoveAllWaggons { get; set; }
        
        /// <summary>
        /// Removes Breaks on saving
        /// </summary>
        public bool ShouldRemoveBreaks { get; set; }
    }
}
