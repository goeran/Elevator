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

        public int? StoredCurrentLevel;
        public void SaveCurrentLevel(int number)
        {
            StoredCurrentLevel = number;
        }

        public int? StubGetCurrentLevel;

        public int GetCurrentLevel()
        {
            if (StubGetCurrentLevel.HasValue) return StubGetCurrentLevel.Value;

            throw new InvalidOperationException("GetCurrentLevel is not stubbed!");
        }
    }
}
