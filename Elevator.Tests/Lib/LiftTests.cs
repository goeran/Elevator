using System;
using Elevator.Lib;
using Elevator.Tests.Fakes;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    public class LiftTests
    {
        [TestFixture]
        public class When_creating
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_requires_logger_to_be_specified()
            {
                 new Lift(null, new LevelDataStorageMock());
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_requires_datastorage_to_be_specified()
            {
                new Lift(new LoggerMock(), null);
            }
        }

        [TestFixture]
        public class When_adding_Level : SharedLiftSetup
        {
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
                mocked.logger.Last_entry_should_equals("Level loaded: 1, initial data structure");
            }
        }

        [TestFixture]
        public class When_start : SharedLiftSetup
        {
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
                mocked.storage.StubHasStoredLevelInfoAndReturn(true);
                mocked.storage.StubGetCurrentLevelAndReturn(new Level(2, ""));
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
                lift.AddLevel(new Level(1, "ground level"));
                lift.Start();
                mocked.logger.Should_contain_entry("Elevator started: Current level [Level Number=1, Description=ground level]");
            }
        }

        [TestFixture]
        public class When_start_for_the_first_time : SharedLiftSetup
        {
            [Test]
            public void It_will_store_current_level_info_in_datastorage()
            {
                lift.AddLevel(new Level(1, ""), new Level(2, ""));
                lift.Start();
                mocked.storage.Should_have_saved_current_level(new Level(1, ""));
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
            public void It_will_announce_going_up()
            {
                var level1 = new Level(1, "");
                lift.AddLevel(level1);

                lift.Start();

                mocked.logger.Should_contain_entry("Arrived at [Level Number=1, Description=]");
            }

            [Test]
            public void It_will_not_store_level_if_lift_fails()
            {
                var level1 = new Level(1, "", () => { throw new Exception("Something happened"); });
                lift.AddLevel(level1);

                lift.Start();

                mocked.storage.Should_not_have_saved_current_level();
            }
        }

        [TestFixture]
        public class When_start_for_the_first_time_and_lift_fails : SharedLiftSetup
        {
            [Test]
            public void It_will_announce_the_failed_lift()
            {
                var level1 = new Level(1, "init setup", () => { throw new Exception("Failed because db is down"); });
                lift.AddLevel(level1);

                lift.Start();

                mocked.logger.Should_contain_entry("Failed to lift: Level 1, init setup");
                mocked.logger.Last_entry_should_start_with("System.Exception: Failed because db is down");
            }             
        }

        [TestFixture]
        public class When_start_for_the_nth_time : SharedLiftSetup
        {
            [SetUp]
            public void Setup()
            {
                mocked.storage.StubHasStoredLevelInfoAndReturn(true);
                mocked.storage.StubGetCurrentLevelAndReturn(new Level(0, "init level"));
                lift.AddLevel(new Level(0, "init level"), new Level(1, "first"), new Level(2, "second"));
                lift.Start();
            }

            [Test]
            public void It_will_stay_on_the_current_left()
            {
                Assert.AreEqual(0, lift.CurrentLevel.Number);
            }

            [Test]
            public void It_will_not_store_current_level()
            {
                mocked.storage.Should_not_have_saved_current_level();
            }
        }

        [TestFixture]
        public class When_going_up : SharedLiftSetup
        {
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

                mocked.storage.Should_have_saved_current_level(new Level(2, ""));
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
                mocked.storage.StubGetCurrentLevelAndReturn(level1);
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

                mocked.storage.Should_have_saved_current_level(new Level(1, ""));
            }

            [Test]
            public void It_will_handle_elevators_with_only_one_level()
            {
                var upCallCount = 0;
                var groundLevel = new Level(0, "The only level", () => upCallCount++);
                lift.AddLevel(groundLevel);
                lift.Start();
                mocked.storage.StubHasStoredLevelInfoAndReturn(true);

                lift.Up();

                Assert.AreEqual(1, upCallCount);
            }

            [Test]
            public void It_will_announce_going_up()
            {
                var level1 = new Level(1, "level 1");
                var level2 = new Level(2, "level 2");
                lift.AddLevel(level1, level2);
                lift.Start();

                lift.Up();

                mocked.logger.Should_contain_entry("Arrived at [Level Number=2, Description=level 2]");
            }
        }

        [TestFixture]
        public class When_going_to_the_top : SharedLiftSetup 
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Lift must be started before going up")]
            public void It_requires_lift_to_be_started()
            {
                lift.Top();
            }

            [Test]
            public void It_will_go_up_all_the_levels()
            {
                var level1Visited = 0;
                var level2Visited = 0;
                var level3Visited = 0;
                var level1 = new Level(1, "", () => level1Visited++);
                var level2 = new Level(2, "", () => level2Visited++);
                var level3 = new Level(3, "", () => level3Visited++);
                lift.AddLevel(level1, level2, level3);
                lift.Start();

                lift.Top();

                Assert.AreEqual(1, level1Visited, "Expected level 1 to be visited");
                Assert.AreEqual(1, level2Visited, "Expected level 2 to be visited");
                Assert.AreEqual(1, level3Visited, "Expected level 3 to be visited");
            }
        }

        [TestFixture]
        public class When_going_to_the_top_for_nth_time : SharedLiftSetup
        {
            private int level1VisitedCount, level2VisitedCount, level3VisitedCount, level4VisitedCount;
            private Level level1, level2, level3, level4;

            [SetUp]
            public void Setup()
            {
                level1VisitedCount = 0;
                level2VisitedCount = 0;
                level3VisitedCount = 0;
                level4VisitedCount = 0;
                level1 = new Level(1, "", () => level1VisitedCount++);
                level2 = new Level(2, "", () => level2VisitedCount++);
                level3 = new Level(3, "", () => level3VisitedCount++);
                level4 = new Level(4, "", () => level4VisitedCount++);
                lift.AddLevel(level1, level2, level3, level4);
                mocked.storage.StubHasStoredLevelInfoAndReturn(true);
                mocked.storage.StubGetCurrentLevelAndReturn(level2);
                lift.Start();

                lift.Top();
            }

            [Test]
            public void It_should_not_visit_levels_that_are_already_visited()
            {
                Assert.AreEqual(0, level1VisitedCount);
                Assert.AreEqual(0, level2VisitedCount);
            }

            [Test]
            public void It_should_visit_unvisited_levels()
            {
                Assert.AreEqual(1, level3VisitedCount, "Level 3 should have been visited only once");
                Assert.AreEqual(1, level4VisitedCount, "Level 4 should have been visited only once");
            }

            [Test]
            public void It_should_end_on_top_level()
            {
                Assert.AreEqual(level4, lift.CurrentLevel);
            }

            [Test]
            public void It_should_store_top_level_as_current_level_in_storage()
            {
                mocked.storage.Should_have_saved_current_level(level4);
            }
        }

        [TestFixture]
        public class When_going_up_and_lift_fails : SharedLiftSetup
        {
            [Test]
            public void It_will_announce_the_failed_lift()
            {
                var level1 = new Level(1, "");
                var level2 = new Level(2, "level 2", () => { throw new Exception("Failed because db is down"); });
                lift.AddLevel(level1, level2);
                lift.Start();

                lift.Up();

                mocked.logger.Should_contain_entry("Failed to lift: Level 2, level 2");
                mocked.logger.Last_entry_should_start_with("System.Exception: Failed because db is dow");
            }             
        }

        public class SharedLiftSetup
        {
            protected Lift lift;
            protected LiftWithMocks mocked;

            [SetUp]
            public void Setup()
            {
                mocked = new LiftWithMocks();
                mocked.storage.StubHasStoredLevelInfoAndReturn(false);
                lift = mocked.liftWithFakes;
            }
        }
    }
}
