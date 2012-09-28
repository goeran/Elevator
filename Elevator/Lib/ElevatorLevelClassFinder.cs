using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elevator.Lib.Internal;

namespace Elevator.Lib
{
    public class ElevatorLevelClassFinder
    {
        public IEnumerable<Type> Find(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException();

            var allClasses = assembly.GetTypes();

            return (from aClass in allClasses
                    let classMeta = new ClassMetadata(aClass)
                    where classMeta.NameIgnoringCaseStartsWith("elevatorlevel") && 
                        classMeta.HasPublicConstructorWithZeroParameters() && 
                        classMeta.HasField("level") && 
                        classMeta.HasField("description") && 
                        classMeta.HasMethod("up")
                    select aClass).ToList();
        }
    }
}