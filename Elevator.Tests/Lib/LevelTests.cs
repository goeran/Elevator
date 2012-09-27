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

        [TestFixture]
        public class When_checking_equality
        {
            [Test]
            public void It_is_equal_when_id_is_equal()
            {
                var level1 = new Level(1, "");
                var level1Copy = new Level(1, "");

                Assert.IsTrue(level1.Equals(level1Copy));
                Assert.AreEqual(level1Copy, level1);
            }

            [Test]
            public void It_support_equality_checking_using_the_operator()
            {
                var level1 = new Level(1, "");
                var level1Copy = new Level(1, "");
                var level2 = new Level(2, "");
                Level nullLevel = null;

                Assert.IsTrue(level1 == level1Copy, "Expected that equality was checked on values - not reference");
                Assert.IsFalse(level1 == level2);
                Assert.IsFalse(level1 == null, "should be possible to check equality with null obj");
                Assert.IsFalse(null == level1, "should be possible to check equality with null obj");
                Assert.IsTrue(nullLevel == null, "Expected to be possible to check for null objects");
                Assert.IsTrue(null == nullLevel, "Expected to be possible to check for null objects");
            }

            [Test]
            public void It_ignore_comment_when_checking_equality()
            {
                var level1 = new Level(1, "level 1");
                var level1Copy = new Level(1, "level 1 copy");

                Assert.IsTrue(level1.Equals(level1Copy));
                Assert.AreEqual(level1Copy, level1);
            }
        }

    }
}
