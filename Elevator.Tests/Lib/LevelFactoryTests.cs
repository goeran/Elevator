using System;
using Elevator.Lib;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    class LevelFactoryTests
    {
        [TestFixture]
        public class When_creting_Level_object
        {
            private LevelFactory levelFactory;

            [SetUp]
            public void Setup()
            {
                levelFactory = new LevelFactory();
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void It_will_throw_exception_if_arg_is_null_obj()
            {
                levelFactory.NewLevel(null);
            }

            [Test]
            [ExpectedException(
                typeof(ArgumentException), 
                ExpectedMessage = "Class 'Object' is missing public instance method named 'Up'")
            ]
            public void It_requires_the_class_have_a_public_instance_method_named_up()
            {
                levelFactory.NewLevel(typeof(Object));
            }

            [Test]
            [ExpectedException(
                typeof(ArgumentException), 
                ExpectedMessage = "Class 'CustomElevatorLevelWithPrivateConstructor' is missing a public constructor with zero args")
            ]
            public void It_requires_the_class_to_have_a_public_constructor()
            {
                levelFactory.NewLevel(typeof (CustomElevatorLevelWithPrivateConstructor));
            }

            [Test]
            [ExpectedException(
                typeof(ArgumentException),
                ExpectedMessage = "Failed to create an instance of 'CustomElevatorLevelWithError'. Check the code in the public constructor with zero args")]
            public void It_requires_that_the_public_constructor_will_not_throw_an_exception()
            {
                levelFactory.NewLevel(typeof (CustomElevatorLevelWithError));
            }

            [Test]
            public void It_will_extract_up_method()
            {
                var level = levelFactory.NewLevel(typeof (CustomElevatorLevel));
                level.Up();
                Assert.AreEqual(1, CustomElevatorLevel.UpCallCount);
            }
        }
    }

    internal class CustomElevatorLevelWithError
    {
        public CustomElevatorLevelWithError()
        {
            throw new Exception("Something bad happened");
        }

        public void Up()
        {
        }
    }

    internal class CustomElevatorLevel
    {
        public static int UpCallCount;

        public void Up()
        {
            UpCallCount++;
        }
    }

    internal class CustomElevatorLevelWithPrivateConstructor
    {
        private CustomElevatorLevelWithPrivateConstructor()
        {
        }
 
        public void Up()
        {
        }
    }
}
