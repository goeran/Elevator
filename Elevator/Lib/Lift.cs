using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Lib
{
    public class Lift
    {
        private readonly SortedList<int, Level> levels = new SortedList<int, Level>();

        public Lift(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException();

            logger.Log("Elevator initialized");
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

            CurrentLevel = levels.First().Value;
        }

        public void Up()
        {
            if (!levels.Any()) throw new InvalidOperationException("No levels exists");

            var index = levels.IndexOfValue(CurrentLevel);
            if (index >= levels.Count - 1) throw new InvalidOperationException("On the top level");

            CurrentLevel = levels.ElementAt(++index).Value;
        }
    }
}