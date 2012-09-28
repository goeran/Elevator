using System;
using System.Linq;
using System.Reflection;

namespace Elevator.Lib.Internal
{
    internal class ClassMetadata
    {
        private const BindingFlags publicInstanceMembers = BindingFlags.Instance | BindingFlags.Public;
        private const BindingFlags allInstanceMembers = publicInstanceMembers | BindingFlags.NonPublic;

        private readonly ConstructorInfo[] publicConstructors;
        private readonly FieldInfo[] allInstanceFields;
        private readonly MethodInfo[] allInstanceMethods;
        private readonly Type aClass;

        public ClassMetadata(Type aClass)
        {
            this.aClass = aClass;
            publicConstructors = aClass.GetConstructors(publicInstanceMembers);
            allInstanceFields = aClass.GetFields(allInstanceMembers);
            allInstanceMethods = aClass.GetMethods(allInstanceMembers);
        }

        public bool HasPublicConstructorWithZeroParameters()
        {
            return publicConstructors.Any(c => !c.GetParameters().Any());
        }

        public bool HasMethod(string name)
        {
            return allInstanceMethods.Any(WhereMethodIgnoringCaseIsNamed("up"));
        }

        private static Func<MethodInfo, bool> WhereMethodIgnoringCaseIsNamed(string name)
        {
            return m => m.Name.ToLower() == name;
        }

        public bool HasField(string name)
        {
            return allInstanceFields.Any(WhereFieldIgnoringCaseIsNamed(name));
        }

        private static Func<FieldInfo, bool> WhereFieldIgnoringCaseIsNamed(string name)
        {
            return f => f.Name.ToLower() == name;
        }

        public bool NameIgnoringCaseStartsWith(string className)
        {
            return aClass.Name.ToLower().StartsWith(className.ToLower());
        }

        public MethodInfo MethodInfo(string name)
        {
            return allInstanceMethods.Single(WhereMethodIgnoringCaseIsNamed(name));
        }

        public FieldInfo FieldInfo(string name)
        {
            return allInstanceFields.Single(WhereFieldIgnoringCaseIsNamed(name));
        }
    }
}
