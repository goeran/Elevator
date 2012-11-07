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


        private bool? stubHasStoredLevelInfo;
        public void StubStoredLevelInfoAndReturn(bool val)
        {
            stubHasStoredLevelInfo = val;
        }
        public bool HasStoredLevelInfo()
        {
            if (stubHasStoredLevelInfo == null) throw new Exception("HasStoredLevelInfo() must be stubbed");
            return stubHasStoredLevelInfo.Value;
        }

        private Level storedCurrentLevel;
        public void SaveCurrentLevel(Level level)
        {
            storedCurrentLevel = level;
        }

        public void Should_have_saved_current_level(Level expectedLevel)
        {
            if (storedCurrentLevel == null) throw new Exception("No level stored yet");
            var errorMessage = string.Format("Expected {0}, but was {1}", expectedLevel, storedCurrentLevel);

            if (!storedCurrentLevel.Equals(expectedLevel)) throw new Exception(errorMessage);
        }

        public void Should_not_have_saved_current_level()
        {
            var storedCurrentLevelAsString = storedCurrentLevel == null ? string.Empty : storedCurrentLevel.ToString();
            var errorMessage = string.Format("Didn't expect current level to be stored, but was {0}", storedCurrentLevelAsString);
            if (storedCurrentLevel != null) throw new Exception(errorMessage);
        }

        public Level StubGetCurrentLevel;
        public Level GetCurrentLevel()
        {
            if (StubGetCurrentLevel != null) return StubGetCurrentLevel;

            throw new Exception("GetCurrentLevel() must be stubbed");
        }

        public void Initialize()
        {

        }
    }
}
