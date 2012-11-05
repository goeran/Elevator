using System;
using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class FakeLevelDataStorage : ILevelDataStorage
    {
        public static int NumberOfInstancesCreated;

        public FakeLevelDataStorage()
        {
            NumberOfInstancesCreated++;
        }

        public bool? StubHasStoredLevelInfo;

        public bool HasStoredLevelInfo()
        {
            if (StubHasStoredLevelInfo.HasValue) return StubHasStoredLevelInfo.Value;

            return StoredCurrentLevel != null;
        }

        public Level StoredCurrentLevel;
        public void SaveCurrentLevel(Level level)
        {
            StoredCurrentLevel = level;
        }

        public Level StubGetCurrentLevel;
        public Level GetCurrentLevel()
        {
            if (StubGetCurrentLevel != null) return StubGetCurrentLevel;

            return StoredCurrentLevel;

        }

        public void Initialize()
        {

        }
    }
}
