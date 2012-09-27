﻿using System;

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
