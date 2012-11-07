using System;
using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class LevelDataStorageMock : ILevelDataStorage
    {
        public static int NumberOfInstancesCreated;

        public LevelDataStorageMock()
        {
            NumberOfInstancesCreated++;
        }

        public void StubStoredLevelInfoAndReturn(bool val)
        {
            stubHasStoredLevelInfo = val;
        }

        private bool? stubHasStoredLevelInfo;
        public bool HasStoredLevelInfo()
        {
            if (stubHasStoredLevelInfo == null) throw new Exception("HasStoredLevelInfo must be stubbed");
            return stubHasStoredLevelInfo.Value;
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
