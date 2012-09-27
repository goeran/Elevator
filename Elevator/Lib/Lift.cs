using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Lib
{
    public class Lift
    {
        private readonly SortedList<int, Level> levels = new SortedList<int, Level>();
        private bool isStarted;
        private ILogger logger;
        private IDataStorage dataStorage;

        public Lift(ILogger logger, IDataStorage dataStorage)
        {
            if (logger == null || dataStorage == null) throw new ArgumentNullException();
            this.logger = logger;
            this.dataStorage = dataStorage;
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
            Announce("Level added: {0}, {1}", level.Number, level.Comment);
        }

        public void Start()
        {
            if (!levels.Any()) throw new InvalidOperationException("At least one level must be specified before the lift can be started");

            isStarted = true;
            Announce("Elevator started");
            CurrentLevel = levels.First().Value;
            Announce("Current level: {0}, {1}", CurrentLevel.Number, CurrentLevel.Comment);
        }

        public void Up()
        {
            if (!isStarted) throw new InvalidOperationException("Lift must be started before going up");

            var index = levels.IndexOfValue(CurrentLevel);
            if (index >= levels.Count - 1) throw new InvalidOperationException("On the top level");
            CurrentLevel = levels.ElementAt(++index).Value;
        }
    }
}