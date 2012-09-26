using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Lib
{
    public class Lift
    {
        private readonly List<Level> levels = new List<Level>();

        public Lift(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException();

            logger.Log("Elevator initialized");
        }

        public void AddLevel(Level level)
        {
            if (level == null) throw new ArgumentNullException();
            if (levels.Any(l => l.Number == level.Number)) throw new ArgumentException("Level already exists");

            levels.Add(level);
        }
    }
}