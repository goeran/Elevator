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
                 new Lift(null);
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
                lift = new Lift(fakeLogger);
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

            [SetUp]
            public void Setup()
            {
                fakeLogger = new FakeLogger();
                lift = new Lift(fakeLogger);
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
        public class When_going_up
        {
            private Lift lift;

            [SetUp]
            public void Setup()
            {
                lift = new Lift(new FakeLogger());
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
        }
    }
}
