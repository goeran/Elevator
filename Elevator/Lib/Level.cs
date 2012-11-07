using System;

namespace Elevator.Lib
{
    public class Level
    {
        private readonly Action up;

        public Level(int number, string description) : this(number, description, () => {})
        {
        }

        public Level(int number, string description, Action up)
        {
            if (description == null || up == null) 
                throw new ArgumentNullException();

            Number = number;
            Description = description;
            this.up = up;
        }

        public int Number { get; private set; }
        public string Description { get; private set; }

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

        public override string ToString()
        {
            return string.Format("[Level Number={0}, Description={1}]", Number, Description);
        }
    }
}
