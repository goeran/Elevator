using System;
using System.Linq;
using System.Reflection;

namespace Elevator.Lib
{
    public class LevelFactory
    {
        public Level NewLevel(Type classMetadata)
        {
            if (classMetadata == null) throw new ArgumentNullException();

            var constructors = classMetadata.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var publicConstructor = constructors.FirstOrDefault();
            if (publicConstructor == null)
                throw new ArgumentException(string.Format("Class '{0}' is missing a public constructor with zero args", classMetadata.Name));

            var publicInstanceMethods = classMetadata.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var upMethod = publicInstanceMethods.SingleOrDefault(m => m.Name.ToLower() == "up");
            if (upMethod == null) 
                throw new ArgumentException(string.Format("Class '{0}' is missing public instance method named 'Up'", classMetadata.Name));

            var instance = Activator.CreateInstance(classMetadata);
            Action upDelegate = () => upMethod.Invoke(instance, null);

            return new Level(1, "", upDelegate);
        }
    }
}