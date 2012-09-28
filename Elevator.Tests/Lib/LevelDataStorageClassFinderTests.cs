using System;
using System.Linq;
using System.Reflection;
using Elevator.Lib;
using Elevator.Tests.Fakes;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    public class LevelDataStorageClassFinderTests
    {
        [TestFixture]
        public class When_searching_for_all_LevelDataStorage_classes_in_assembly
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_throws_exception_if_args_is_nullobj()
            {
                new LevelDataStorageClassFinder().Find(null);
            }

            [Test]
            public void It_will_find_classes_data_implement_the_ILevelDataStorage_interface()
            {
                var classFinder = new LevelDataStorageClassFinder();
                var levelDataStorageClasses = classFinder.Find(Assembly.GetExecutingAssembly());
                Assert.Contains(typeof(FakeLevelDataStorage), levelDataStorageClasses.ToList());
            }
        }

    }
}
