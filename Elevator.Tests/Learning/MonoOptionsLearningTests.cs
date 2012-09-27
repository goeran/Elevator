﻿using Mono.Options;
using NUnit.Framework;

namespace Elevator.Tests.Learning
{
    [TestFixture]
    public class MonoOptionsLearningTests
    {
        [Test]
        public void How_to_parse_using_optionSet()
        {
            var config = new AConfiguration();
            
            var args = new[] {"--name=Gøran", "/age:31", "something bla"};
            var optionSet = new OptionSet();
            optionSet.Add("name=", "Parameter is needed", option =>
            {
                config.Name = option;
            });
            optionSet.Add("age:", option =>
            {
                config.Age = int.Parse(option);
            });
            optionSet.Parse(args);

            Assert.AreEqual("Gøran", config.Name);
            Assert.AreEqual(31, config.Age);
        }

        class AConfiguration
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
