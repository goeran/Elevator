using System;
using System.Linq;
using Elevator.Lib;
using Elevator.Tests.Fakes;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    class LiftTests
    {
        [TestFixture]
        public class When_creating
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_requires_logger_to_be_specified()
            {
                 new Lift(null, new FakeLevelDataStorage());
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_requires_datastorage_to_be_specified()
            {
                new Lift(new FakeLogger(), null);
            }
        }

        [TestFixture]
        public class When_adding_Level
        {
            private Lift lift;
            private FakeLogger fakeLogger;

            [SetUp]
            public void Setup()
            {
                fakeLogger = new FakeLogger();
                lift = new Lift(fakeLogger, new FakeLevelDataStorage());
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_throws_exception_if_null_obj()
            {
                lift.AddLevel((Level)null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Level already exists")]
            public void It_will_check_if_the_level_already_exists()
            {
                lift.AddLevel(new Level(1, "initial structure"));
                lift.AddLevel(new Level(1, "dining"));
            }

            [Test]
            public void It_will_announce_Level_was_added()
            {
                lift.AddLevel(new Level(1, "initial data structure"));

                Assert.AreEqual("Level added: 1, initial data structure", fakeLogger.LastEntry);
            }
        }

        [TestFixture]
        public class When_start
        {
            private FakeLogger fakeLogger;
            private Lift lift;
            private FakeLevelDataStorage fakeLevelDataStorage;

            [SetUp]
            public void Setup()
            {
                fakeLogger = new FakeLogger();
                fakeLevelDataStorage = new FakeLevelDataStorage();
                fakeLevelDataStorage.StubHasStoredLevelInfo = false;
                lift = new Lift(fakeLogger, fakeLevelDataStorage);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "At least one level must be specified before the lift can be started")]
            public void It_requires_at_least_one_level_bare_it_can_be_started()
            {
                lift.Start();
            }

            [Test]
            public void It_will_set_current_level()
            {
                var level1 = new Level(1, "ground level");
                lift.AddLevel(level1);

                lift.Start();

                Assert.AreEqual(level1, lift.CurrentLevel);
            }

            [Test]
            public void It_will_set_the_current_level_based_on_current_level_stored_in_datastorage()
            {
                fakeLevelDataStorage.StubHasStoredLevelInfo = true;
                fakeLevelDataStorage.StubGetCurrentLevel = new Level(2, "");
                lift.AddLevel(new Level(1, ""), new Level(2, ""), new Level(3, ""));
                
                lift.Start();

                Assert.AreEqual(2, lift.CurrentLevel.Number);
            }

            [Test]
            public void It_will_update_current_level_to_be_the_lowest_level()
            {
                var levelMinus1 = new Level(-1, "");
                var level1 = new Level(1, "");
                var level2 = new Level(2, "");
                lift.AddLevel(levelMinus1, level1, level2);

                lift.Start();

                Assert.AreEqual(levelMinus1, lift.CurrentLevel);
            }

            [Test]
            public void It_will_announce_that_it_has_started()
            {
                lift.AddLevel(new Level(1, ""));
                lift.Start();
                Assert.AreEqual("Elevator started", fakeLogger.Entries[fakeLogger.Entries.Count - 2]);
            }

            [Test]
            public void It_will_announce_current_level()
            {
                lift.AddLevel(new Level(1, "ground level"));
                lift.Start();
                Assert.AreEqual("Current level: 1, ground level", fakeLogger.LastEntry);
            }
        }

        [TestFixture]
        public class When_start_for_the_first_time
        {
            private FakeLevelDataStorage fakeLevelDataStorage;
            private Lift lift;

            [SetUp]
            public void Setup()
            {
                fakeLevelDataStorage = new FakeLevelDataStorage();
                fakeLevelDataStorage.StubHasStoredLevelInfo = false;
                lift = new Lift(new FakeLogger(), fakeLevelDataStorage);
            }

            [Test]
            public void It_will_store_current_level_info_in_datastorage()
            {
                lift.AddLevel(new Level(1, ""), new Level(2, ""));
                lift.Start();
                Assert.AreEqual(new Level(1, ""), fakeLevelDataStorage.StoredCurrentLevel);
            }

            [Test]
            public void It_will_call_up_on_the_level_object()
            {
                var hasCalledUpDelegate = false;
                var level1 = new Level(1, "", () => { hasCalledUpDelegate = true; });
                lift.AddLevel(level1);

                lift.Start();

                Assert.IsTrue(hasCalledUpDelegate, "Expected up to be called on level object");
            }

            [Test]
            public void It_will_not_store_level_if_lift_fails()
            {
                var level1 = new Level(1, "", () => { throw new Exception("Something happened"); });
                lift.AddLevel(level1);

                lift.Start();

                Assert.IsNull(fakeLevelDataStorage.StoredCurrentLevel, "Current level should not have been stored");
            }
        }

        [TestFixture]
        public class When_start_for_the_first_time_and_lift_fails
        {
            [Test]
            public void It_will_announce_the_failed_lift()
            {
                var level1 = new Level(1, "init setup", () => { throw new Exception("Failed because db is down"); });
                var fakeLogger = new FakeLogger();
                var fakeLevelDataStorage = new FakeLevelDataStorage();
                fakeLevelDataStorage.StubHasStoredLevelInfo = false;
                var lift = new Lift(fakeLogger, fakeLevelDataStorage);
                lift.AddLevel(level1);

                lift.Start();

                Assert.AreEqual("Failed to lift: Level 1, init setup", fakeLogger.Entries[fakeLogger.Entries.Count - 2]);
                Assert.IsTrue(fakeLogger.LastEntry.Contains(new Exception("Failed because db is down").ToString()));
            }             
        }

        [TestFixture]
        public class When_going_up
        {
            private Lift lift;
            private FakeLevelDataStorage fakeLevelDataStorage;

            [SetUp]
            public void Setup()
            {
                fakeLevelDataStorage = new FakeLevelDataStorage();
                fakeLevelDataStorage.StubHasStoredLevelInfo = false;
                lift = new Lift(new FakeLogger(), fakeLevelDataStorage);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Lift must be started before going up")]
            public void It_requires_lift_to_be_started_before_going_up()
            {
                lift.AddLevel(new Level(1, ""), new Level(2, ""));
                lift.Up();
            }

            [Test]
            public void It_will_go_to_next_level()
            {
                var level1 = new Level(1, "");
                var level2 = new Level(2, "");
                lift.AddLevel(level1, level2);
                lift.Start();

                lift.Up();

                Assert.AreEqual(level2, lift.CurrentLevel);
            }

            [Test]
            public void It_will_store_current_level_info_in_datastorage()
            {
                lift.AddLevel(new Level(1, ""), new Level(2, ""), new Level(3, ""));
                lift.Start();

                lift.Up();

                Assert.IsTrue(fakeLevelDataStorage.StoredCurrentLevel != null, "Expected current level number to be stored");
                Assert.AreEqual(new Level(2, ""), fakeLevelDataStorage.StoredCurrentLevel);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "On the top level")]
            public void It_will_throw_exception_when_going_over_top_level()
            {
                var level1 = new Level(1, "");
                var level2 = new Level(2, "");
                lift.AddLevel(level1, level2);
                lift.Start();

                lift.Up();
                lift.Up();
            }

            [Test]
            public void It_will_call_up_on_the_level_object()
            {
                var hasCalledUpDelegate = false;
                var level1 = new Level(1, "");
                var level2 = new Level(2, "", () => { hasCalledUpDelegate = true; });
                lift.AddLevel(level1, level2);
                lift.Start();

                lift.Up();

                Assert.IsTrue(hasCalledUpDelegate, "Expected up to be called on level object");
            }


            [Test]
            public void It_will_not_store_level_if_lift_fails()
            {
                var level1 = new Level(1, "");
                var level2 = new Level(2, "", () => { throw new Exception("Could not lift"); });
                lift.AddLevel(level1, level2);
                lift.Start();

                lift.Start();

                Assert.AreNotEqual(level2, fakeLevelDataStorage.StoredCurrentLevel);
                Assert.AreEqual(level1, fakeLevelDataStorage.StoredCurrentLevel);
            }
        }
    }
}
