using System;
using Elevator.Lib;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    class LevelTests
    {
        [TestFixture]
        public class When_creating
        {
            [Test]
            public void It_requires_a_number()
            {
                new Level(1, "");
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_requires_a_description()
            {
                new Level(1, null);
            }
        }

        [TestFixture]
        public class When_created
        {
            [Test]
            public void It_should_set_Number_property()
            {
                var level = new Level(1, "description");
                Assert.AreEqual(1, level.Number);
            }

            [Test]
            public void It_should_set_Description_property()
            {
                var level = new Level(1, "initial schema");
                Assert.AreEqual("initial schema", level.Comment);
            }
        }
    }
}
