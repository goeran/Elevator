using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Lib
{
    public class Lift
    {
        private readonly SortedList<int, Level> levels = new SortedList<int, Level>();
        private bool isStarted;
        private readonly ILogger logger;
        private readonly ILevelDataStorage levelDataStorage;

        public Lift(ILogger logger, ILevelDataStorage levelDataStorage)
        {
            if (logger == null || levelDataStorage == null) throw new ArgumentNullException();
            this.logger = logger;
            this.levelDataStorage = levelDataStorage;
        }

        private void Announce(string message, params object[] objects)
        {
            logger.Log(string.Format(message, objects));
        }

        public Level CurrentLevel { get; private set; }

        public void AddLevel(params Level[] newLevels)
        {
            foreach (var newLevel in newLevels)
            {
                AddLevel(newLevel);
            }
        }

        public void AddLevel(Level level)
        {
            if (level == null) throw new ArgumentNullException();
            if (levels.ContainsKey(level.Number)) throw new ArgumentException("Level already exists");

            levels.Add(level.Number, level);
            Announce("Level loaded: {0}, {1}", level.Number, level.Description);
        }

        public void Start()
        {
            if (!levels.Any()) throw new InvalidOperationException("At least one level must be specified before the lift can be started");

            isStarted = true;
            if (levelDataStorage.HasStoredLevelInfo())
            {
                var storedLevel = levelDataStorage.GetCurrentLevel();
                CurrentLevel = levels[storedLevel.Number];
                AnnounceStart();
            }
            else
            {
                CurrentLevel = levels.First().Value;
                AnnounceStart();
                LiftUp();
            }
        }

        private void AnnounceStart()
        {
            Announce("Elevator started: Current level {0}", CurrentLevel);
        }

        public void Up()
        {
            GuardToMakeSureLiftIsStarted();
            PrepareLiftToNextLevel();
            LiftUp();
        }

        private void GuardToMakeSureLiftIsStarted()
        {
            if (!isStarted) throw new InvalidOperationException("Lift must be started before going up");
        }

        private void PrepareLiftToNextLevel()
        {
            var currentLevelNumber = levels.IndexOfValue(CurrentLevel);
            if (levels.Count > 1 && currentLevelNumber >= levels.Count - 1) throw new InvalidOperationException("On the top level");
            var newCurrentLevelNumber = ++currentLevelNumber;
            if (newCurrentLevelNumber < levels.Count)
            {
                CurrentLevel = levels.ElementAt(newCurrentLevelNumber).Value;
            }
        }

        private void LiftUp()
        {
            try
            {
                if (!levelDataStorage.HasStoredLevelInfo() || 
                    CurrentLevel.Number > levelDataStorage.GetCurrentLevel().Number)
                {
                    CurrentLevel.Up();
                    StoreCurrentLevelInfo();
                    Announce("Arrived at {0}", CurrentLevel);
                }
            }
            catch (Exception ex)
            {
                Announce("Failed to lift: Level {0}, {1}", CurrentLevel.Number, CurrentLevel.Description);
                Announce(ex.ToString());
            }
        }

        private void StoreCurrentLevelInfo()
        {
            levelDataStorage.SaveCurrentLevel(CurrentLevel);
        }

        public void Top()
        {
            GuardToMakeSureLiftIsStarted();

            while (CurrentLevel.Number < TopLevel().Number)
            {
                Up();
            }
        }

        private Level TopLevel()
        {
            return levels.Last().Value;
        }
    }
}