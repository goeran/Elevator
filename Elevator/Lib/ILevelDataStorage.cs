namespace Elevator.Lib
{
    public interface ILevelDataStorage
    {
        void Initialize();
        bool HasStoredLevelInfo();
        void SaveCurrentLevel(Level level);
        Level GetCurrentLevel();
    }
}
