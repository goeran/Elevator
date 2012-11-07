using System;
using Elevator.Lib;

namespace Elevator.AcceptanceTests.Fakes
{
    class AcceptanceTestFakeLevelDataStorage : ILevelDataStorage
    {
        public static int NumberOfInstancesCreated;

        public AcceptanceTestFakeLevelDataStorage()
        {
            NumberOfInstancesCreated++;
        }

        public void Initialize()
        {
            
        }

        public bool HasStoredLevelInfo()
        {
            return false;
        }

        public void SaveCurrentLevel(Level level)
        {
        }

        public Level GetCurrentLevel()
        {
            throw new NotImplementedException();
        }

    }

}
