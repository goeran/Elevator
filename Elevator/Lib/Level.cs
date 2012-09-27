using System;

namespace Elevator.Lib
{
    public class Level
    {
        private readonly Action up;
        private readonly Action down;

        public Level(int number, string comment) : this(number, comment, () => {}, () => {})
        {
        }

        public Level(int number, string comment, Action up, Action down)
        {
            if (comment == null || up == null && down == null) 
                throw new ArgumentNullException();

            Number = number;
            Comment = comment;
            this.down = down;
            this.up = up;
        }

        public int Number { get; private set; }
        public string Comment { get; private set; }

        public void Up()
        {
            up();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherLevel = obj as Level;
            return otherLevel.Number.Equals(Number);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public static bool operator ==(Level a, Level b)
        {
            //if both are nulls
            if (((object)a == null) && ((object)b == null)) return true;

            //if a is null
            if ((object)a == null) return false;

            //equality handled by equals method
            return a.Equals(b);
        }

        public static bool operator !=(Level a, Level b)
        {
            if ((object)a == null && (object)b != null) return true;
            if ((object)a == null) return false;

            return !a.Equals(b);
        }
    }
}
