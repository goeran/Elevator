namespace Elevator.Lib
{
    public interface ILevelDataStorage
    {
        bool HasStoredLevelInfo();
        void SaveCurrentLevel(int number);
        int GetCurrentLevel();
    }
}
