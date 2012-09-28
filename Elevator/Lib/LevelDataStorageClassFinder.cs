using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elevator.Lib
{
    public class LevelDataStorageClassFinder
    {
        public IEnumerable<Type> Find(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException();

            var classes = assembly.GetTypes();
            var levelDataStorageClasses = classes.Where(c => (typeof(ILevelDataStorage).IsAssignableFrom(c)));

            return levelDataStorageClasses;
        }
    }
}