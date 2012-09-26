using System;

namespace Elevator.Lib
{
    public class Level
    {
        public Level(int number, string comment)
        {
            if (comment == null) throw new ArgumentNullException();

            Number = number;
            Comment = comment;
        }

        public int Number { get; private set; }
        public string Comment { get; private set; }
    }
}
