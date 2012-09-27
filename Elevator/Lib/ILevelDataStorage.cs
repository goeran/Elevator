namespace Elevator.Lib
{
    public interface ILevelDataStorage
    {
        bool HasStoredLevelInfo();
        void SaveCurrentLevel(Level level);
        Level GetCurrentLevel();
    }
}
