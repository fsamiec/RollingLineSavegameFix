namespace RollingLineSavegameFix.Services
{
    /// <summary>
    /// Service to manipulate Savegames
    /// </summary>
    public interface ISavegameService
    {
        /// <summary>
        /// Loads a Savegame
        /// </summary>
        /// <returns>Error Message trying to read the file</returns>
        string LoadSavegame();

        /// <summary>
        /// Tries to Write a new Savefile
        /// </summary>
        /// <returns>Filename to new Savegame</returns>
        void WriteNewSaveGame();
    }
}
