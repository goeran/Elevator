﻿using Elevator.AcceptanceTests.Fakes;
using Elevator.Tests.Fakes;
using NUnit.Framework;

namespace Elevator.AcceptanceTests
{
    class ConsoleTests
    {
        [TestFixture]
        public class When_running_without_specifying_args
        {
            private LoggerMock fakeLogger;

            [SetUp]
            public void Setup()
            {
                fakeLogger = new LoggerMock();
                Shell.LoggerFactory = () => fakeLogger;
            }

            [Test]
            public void It_will_print_help_for_direction()
            {
                Shell.Run();

                fakeLogger.Should_contain_entry("You have to specify direction:");
                fakeLogger.Should_contain_entry("\tElevator.exe -up, for going up");
                fakeLogger.Last_entry_should_equals("\tElevator.exe -down, for going down (not supported yet)");
            }

            [Test]
            public void It_will_print_help_for_migration_assembly()
            {
                Shell.Run("-up");

                fakeLogger.Should_contain_entry("You have to specify migration assembly:");
                fakeLogger.Last_entry_should_equals("\tElevator.exe -up -assembly:name.dll");
            }
        }

        [TestFixture]
        public class When_running_with_valid_args
        {
            private LoggerMock fakeLogger;

            [SetUp]
            public void Setup()
            {
                fakeLogger = new LoggerMock();
                Shell.LoggerFactory = () => fakeLogger;
            }

            [Test]
            public void It_will_create_a_LevelDataStorage_object_by_searching_for_classes_in_the_named_assembly()
            {
                Shell.Run("-up", "-assembly:Elevator.AcceptanceTests.dll");
                Assert.AreEqual(1, AcceptanceTestFakeLevelDataStorage.NumberOfInstancesCreated, "Expected LevelDataStorage object to be created");
            }

            [Test]
            public void It_will_print_help_if_no_LevelDataStorage_class_is_found_in_named_assembly()
            {
                Shell.Run("-up", "-assembly:Tests.Empty.dll");
                fakeLogger.Last_entry_should_equals("Could not find a class in assembly 'Tests.Empty.dll' that implements the ILevelDataStorage interface.");
            }
        }
    }
}
