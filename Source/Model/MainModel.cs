namespace RollingLineSavegameFix.Model
{

    public class MainModel : IMainModel
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
        /// Indicates if Objects on a Map will be moved
        /// </summary>
        public bool ShouldMoveObjects { get; set; }

        /// <summary>
        /// Moves on X Axis
        /// </summary>
        public float MoveXAxisValue { get; set; }

        /// <summary>
        /// Moves on Y Axis
        /// </summary>
        public float MoveYAxisValue { get; set; }

        /// <summary>
        /// Moves on Z Axis
        /// </summary>
        public float MoveZAxisValue { get; set; }
    }
}
