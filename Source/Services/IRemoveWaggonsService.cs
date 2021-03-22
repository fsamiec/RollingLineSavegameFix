namespace RollingLineSavegameFix.Services
{
    public interface IRemoveWaggonsService
    {
        void RemoveAllWaggons();
        void RemoveFaultyQuickmodWaggons();
    }
}

