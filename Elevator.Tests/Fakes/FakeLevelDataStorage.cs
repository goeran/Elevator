using System;
using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class FakeLevelDataStorage : ILevelDataStorage
    {
        public bool? StubHasStoredLevelInfo;
        public bool HasStoredLevelInfo()
        {
            if (StubHasStoredLevelInfo.HasValue) return StubHasStoredLevelInfo.Value;

            throw new InvalidOperationException("HasStoredLevelInfo is not stubbed!");
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

            throw new InvalidOperationException("GetCurrentLevel is not stubbed!");
        }
    }
}
