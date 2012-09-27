using System;
using System.Linq;
using System.Reflection;
using Elevator.Lib;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    class ElevatorLevelClassFinderTests
    {
        [TestFixture]
        public class When_search_for_all_ElevatorLevel_Classes_in_an_assembly
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_throws_exception_if_args_is_a_nullobj()
            {
                new ElevatorLevelClassFinder().Find(null);
            }

            [Test]
            public void It_will_find_all_classes_that_adheres_to_naming_convention()
            {
                var finder = new ElevatorLevelClassFinder();
                var elevatorClasses = finder.Find(Assembly.GetExecutingAssembly());
                Assert.Contains(typeof(ElevatorLevel1), elevatorClasses.ToList());
                Assert.IsFalse(elevatorClasses.Contains(typeof(ElevatorLevelThatShouldBeIgnored)));
            }
        }
    }

    public class ElevatorLevel1
    {
        string Level = "1";
        string description = "Initial data structure";
        void Up()
        {
        }
    }

    public class ElevatorLevelThatShouldBeIgnored
    {
        private ElevatorLevelThatShouldBeIgnored()
        {
        }

        public ElevatorLevelThatShouldBeIgnored(string name)
        {
        }

        int Level = 2;
        string description = "updating field";
        void Up()
        {
        }
    }
}
