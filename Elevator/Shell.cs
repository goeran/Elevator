using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Elevator.Lib;
using Elevator.Lib.Internal;
using Mono.Options;

namespace Elevator
{
    public class Shell
    {
        public static Func<ILogger> LoggerFactory = () => new ConsoleLogger();

        public static void Run(params string[] args)
        {
            var logger = LoggerFactory();

            var config = new ElevatorConfiguration();
            var optionSet = new OptionSet();
            optionSet.Add("up", option =>
            {
                config.Direction = option;
            });
            optionSet.Add("assembly=", option =>
            {
                config.AssemblyIsSpecified = true;
                config.AssemblyName = option;
            });
            optionSet.Parse(args);

            if (string.IsNullOrEmpty(config.Direction))
            {
                logger.Log("You have to specify direction:");
                logger.Log("\tElevator.exe -up, for going up");
                logger.Log("\tElevator.exe -down, for going down (not supported yet)");
            }
            else if (!config.AssemblyIsSpecified)
            {
                logger.Log("You have to specify migration assembly:");
                logger.Log("\tElevator.exe -up -assembly:name.dll");
            }

            if (config.IsValid())
            {
                var migrationAssembly = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, config.AssemblyName));
                var levelDataStorageClasses = new LevelDataStorageClassFinder().Find(migrationAssembly);
                Activator.CreateInstance(levelDataStorageClasses.First());
            }
        }

        class ElevatorConfiguration
        {
            public string Direction { get; set; }
            public bool AssemblyIsSpecified { get; set; }
            public string AssemblyName { get; set; }

            public bool IsValid()
            {
                return !string.IsNullOrEmpty(Direction) &&
                       AssemblyIsSpecified;
            }
        }
    }
}
