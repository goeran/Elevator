using Elevator.Tests.Fakes;
using NUnit.Framework;

namespace Elevator.AcceptanceTests
{
    class ConsoleTests
    {
        [TestFixture]
        public class When_running_without_specifying_args
        {
            private FakeLogger fakeLogger;

            [SetUp]
            public void Setup()
            {
                fakeLogger = new FakeLogger();
                Shell.LoggerFactory = () => fakeLogger;
            }

            [Test]
            public void It_will_print_help_for_direction()
            {
                Shell.Run();

                Assert.AreEqual("You have to specify direction:", fakeLogger.LastEntry(-2));
                Assert.AreEqual("\tElevator.exe -up, for going up", fakeLogger.LastEntry(-1));
                Assert.AreEqual("\tElevator.exe -down, for going down (not supported yet)", fakeLogger.LastEntry());
            }

            [Test]
            public void It_will_print_help_for_migration_assembly()
            {
                Shell.Run("-up");

                Assert.AreEqual("You have to specify migration assembly:", fakeLogger.LastEntry(-1));
                Assert.AreEqual("\tElevator.exe -up -assembly:name.dll", fakeLogger.LastEntry());
            }
        }

        [TestFixture]
        public class When_running_with_valid_args
        {
            private FakeLogger fakeLogger;

            [SetUp]
            public void Setup()
            {
                fakeLogger = new FakeLogger();
                Shell.LoggerFactory = () => fakeLogger;
            }

            [Test]
            public void It_will_create_a_LevelDataStorage_object_by_searching_for_classes_in_the_named_assembly()
            {
                Shell.Run("-up", "-assembly:Elevator.AcceptanceTests.dll");

                Assert.AreEqual(1, FakeLevelDataStorage.NumberOfInstancesCreated, "Expected LevelDataStorage object to be created");
            }

            [Test]
            public void It_will_print_help_if_no_LevelDataStorage_class_is_found_in_named_assembly()
            {
                Shell.Run("-up", "-assembly:Tests.Empty.dll");

                Assert.AreEqual("Could not find a class in assembly 'Tests.Empty.dll' that implements the ILevelDataStorage interface.", fakeLogger.LastEntry());
            }
        }

    }
}
