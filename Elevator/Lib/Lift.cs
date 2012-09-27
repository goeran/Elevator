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
            Announce("Level added: {0}, {1}", level.Number, level.Description);
        }

        public void Start()
        {
            if (!levels.Any()) throw new InvalidOperationException("At least one level must be specified before the lift can be started");

            isStarted = true;
            Announce("Elevator started");
            
            if (levelDataStorage.HasStoredLevelInfo())
            {
                var storedLevel = levelDataStorage.GetCurrentLevel();
                CurrentLevel = levels[storedLevel.Number];
                Announce("Current level: {0}, {1}", CurrentLevel.Number, CurrentLevel.Description);
            }
            else
            {
                CurrentLevel = levels.First().Value;
                Announce("Current level: {0}, {1}", CurrentLevel.Number, CurrentLevel.Description);
                LiftUp();
            }
        }

        public void Up()
        {
            if (!isStarted) throw new InvalidOperationException("Lift must be started before going up");
            PrepareLiftToNextLevel();
            LiftUp();
        }

        private void PrepareLiftToNextLevel()
        {
            var currentLevelNumber = levels.IndexOfValue(CurrentLevel);
            if (currentLevelNumber >= levels.Count - 1) throw new InvalidOperationException("On the top level");
            var newCurrentLevelNumber = ++currentLevelNumber;
            CurrentLevel = levels.ElementAt(newCurrentLevelNumber).Value;
        }

        private void LiftUp()
        {
            try
            {
                CurrentLevel.Up();
                StoreCurrentLevelInfo();
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
    }
}