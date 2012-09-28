using System;
using System.Reflection;
using Elevator.Lib.Internal;

namespace Elevator.Lib
{
    public class LevelFactory
    {
        public Level NewLevel(Type aClass)
        {
            if (aClass == null) throw new ArgumentNullException();

            var classMetadata = new ClassMetadata(aClass);

            if (!classMetadata.HasPublicConstructorWithZeroParameters())
                Throw("Class '{0}' is missing a public constructor with zero args", aClass.Name);
            if (!classMetadata.HasMethod("up")) 
                Throw("Class '{0}' is missing public instance method named 'Up'", aClass.Name);

            if (!classMetadata.HasField("level"))
                Throw("Class '{0}' is missing a public field named 'Level'", aClass.Name);

            int? levelNumber = null;
            Action upDelegate = null;
            string description = string.Empty;

            try
            {
                var instance = Activator.CreateInstance(aClass);
                upDelegate = () => classMetadata.MethodInfo("up").Invoke(instance, null);
                levelNumber = Convert.ToInt32(classMetadata.FieldInfo("level").GetValue(instance));
                if (classMetadata.HasField("description")) 
                    description = Convert.ToString(classMetadata.FieldInfo("description").GetValue(instance));
            }
            catch (TargetInvocationException ex)
            {
                var message = string.Format("Failed to create an instance of '{0}'. Check the code in the public constructor with zero args", aClass.Name);
                throw new ArgumentException(message, ex);
            }

            return new Level(levelNumber.Value, description, upDelegate);
        }

        private static void Throw(string message, params object[] args)
        {
            throw new ArgumentException(string.Format(message, args));
        }
    }
}