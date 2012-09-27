using System;
using System.Linq;
using System.Reflection;

namespace Elevator.Lib.Internal
{
    internal class ElevatorLevelClassMetadata
    {
        private const BindingFlags publicInstanceMembers = BindingFlags.Instance | BindingFlags.Public;
        private const BindingFlags allInstanceMembers = publicInstanceMembers | BindingFlags.NonPublic;

        private readonly ConstructorInfo[] publicConstructors;
        private readonly FieldInfo[] publicInstanceFields;
        private readonly MethodInfo[] publicInstanceMethods;
        private readonly Type aClass;

        public ElevatorLevelClassMetadata(Type aClass)
        {
            this.aClass = aClass;
            publicConstructors = aClass.GetConstructors(publicInstanceMembers);
            publicInstanceFields = aClass.GetFields(allInstanceMembers);
            publicInstanceMethods = aClass.GetMethods(allInstanceMembers);
        }

        public bool HasPublicConstructorWithZeroParameters()
        {
            return publicConstructors.Any(c => !c.GetParameters().Any());
        }

        public bool HasUpMethod()
        {
            return publicInstanceMethods.Any(WhereNameEqualsUp());
        }

        private static Func<MethodInfo, bool> WhereNameEqualsUp()
        {
            return m => m.Name.ToLower() == "up";
        }

        public bool HasDescriptionField()
        {
            return publicInstanceFields.Any(WhereNameEqualsDescription());
        }

        private static Func<FieldInfo, bool> WhereNameEqualsDescription()
        {
            return f => f.Name.ToLower() == "description";
        }

        public bool HasLevelField()
        {
            return publicInstanceFields.Any(WhereNameEqualsLevel());
        }

        private static Func<FieldInfo, bool> WhereNameEqualsLevel()
        {
            return f => f.Name.ToLower() == "level";
        }

        public bool NameIgnoringCaseStartsWith(string className)
        {
            return aClass.Name.ToLower().StartsWith(className.ToLower());
        }

        public MethodInfo UpMethodInfo()
        {
            return publicInstanceMethods.Single(WhereNameEqualsUp());
        }

        public FieldInfo LevelFieldInfo()
        {
            return publicInstanceFields.Single(WhereNameEqualsLevel());
        }

        public FieldInfo DescriptionFieldInfo()
        {
            return publicInstanceFields.Single(WhereNameEqualsDescription());
        }
    }
}
