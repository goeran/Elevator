using System;
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
        public class When_created
        {
            [Test]
            public void It_will_log_initialized()
            {
                var logger = new FakeLogger();
                var lift = new Lift(logger);
                Assert.AreEqual("Elevator initialized", logger.LastEntry);
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
            public void It_will_update_current_level()
            {
                var level1 = new Level(1, "");
                
                lift.AddLevel(level1);

                Assert.AreEqual(level1, lift.CurrentLevel);
            }

            [Test]
            public void It_will_update_current_level_to_be_the_lowest_level()
            {
                var levelMinus1 = new Level(-1, "");
                var level1 = new Level(1, "");
                var level2 = new Level(2, "");
                
                lift.AddLevel(levelMinus1, level1, level2);

                Assert.AreEqual(levelMinus1, lift.CurrentLevel);
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
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "No levels exists")]
            public void It_throw_exception_if_it_doesnt_have_any_levels()
            {
                lift.Up();
            }

            [Test]
            public void It_will_go_to_next_level()
            {
                var level1 = new Level(1, "");
                var level2 = new Level(2, "");
                lift.AddLevel(level1, level2);

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

                lift.Up();
                lift.Up();
            }
        }
    }
}
