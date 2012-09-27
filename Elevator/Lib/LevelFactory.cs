using System;
using System.Linq;
using System.Reflection;

namespace Elevator.Lib
{
    public class LevelFactory
    {
        private const BindingFlags publicInstanceMembers = BindingFlags.Instance | BindingFlags.Public;

        public Level NewLevel(Type classMetadata)
        {
            if (classMetadata == null) throw new ArgumentNullException();

            var constructors = classMetadata.GetConstructors(publicInstanceMembers);
            var publicConstructor = constructors.FirstOrDefault();
            if (publicConstructor == null)
                Throw("Class '{0}' is missing a public constructor with zero args", classMetadata.Name);

            var publicInstanceMethods = classMetadata.GetMethods(publicInstanceMembers);
            var upMethod = publicInstanceMethods.SingleOrDefault(m => m.Name.ToLower() == "up");
            if (upMethod == null) 
                Throw("Class '{0}' is missing public instance method named 'Up'", classMetadata.Name);

            Action upDelegate = null;

            try
            {
                var instance = Activator.CreateInstance(classMetadata);
                upDelegate = () => upMethod.Invoke(instance, null);
            }
            catch (TargetInvocationException ex)
            {
                var message = string.Format("Failed to create an instance of '{0}'. Check the code in the public constructor with zero args", classMetadata.Name);
                throw new ArgumentException(message, ex);
            }

            return new Level(1, "", upDelegate);
        }

        private static void Throw(string message, params object[] args)
        {
            throw new ArgumentException(string.Format(message, args));
        }
    }
}