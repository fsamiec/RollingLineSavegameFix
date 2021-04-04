namespace RollingLineSavegameFix.Model
{
    public interface IMainModel
    {
        string FileName { get; set; }
        string FileContent { get; set; }
        bool ShouldRemoveFaultyWaggons { get; set; }
        bool ShouldRemoveAllWaggons { get; set; }
        bool ShouldMoveObjects { get; set; }
        float MoveXAxisValue { get; set; }
        float MoveYAxisValue { get; set; }
        float MoveZAxisValue { get; set; }
    }    
}
