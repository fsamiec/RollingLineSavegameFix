namespace RollingLineSavegameFix.Model
{
    public interface IMainModel
    {
        string FileName { get; set; }
        string FileContent { get; set; }
        bool ShouldRemoveFaultyWaggons { get; set; }
        bool ShouldRemoveAllWaggons { get; set; }        
    }    
}
