using System;
using System.Collections.Generic;
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
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_throws_exception_if_null_obj()
            {
                var lift = new Lift(new FakeLogger());
                lift.AddLevel(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Level already exists")]
            public void It_will_check_if_the_level_already_exists()
            {
                var lift = new Lift(new FakeLogger());
                lift.AddLevel(new Level(1, "initial structure"));
                lift.AddLevel(new Level(1, "dining"));
            }
        }
    }

    class Lift
    {
        private readonly List<Level> levels = new List<Level>();

        public Lift(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException();

            logger.Log("Elevator initialized");
        }

        public void AddLevel(Level level)
        {
            if (level == null) throw new ArgumentNullException();
            if (levels.Any(l => l.Number == level.Number)) throw new ArgumentException("Level already exists");

            levels.Add(level);
        }
    }

    interface ILogger
    {
        void Log(string message);
    }
}
