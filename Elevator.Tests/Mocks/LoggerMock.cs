using System;
using System.Collections.Generic;
using System.Linq;
using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class LoggerMock : ILogger
    {
        private readonly List<string> entries = new List<string>();
        
        public void Log(string message)
        {
            entries.Add(message);
        }

        public void Should_contain_entry(string expectedEntry)
        {
            var matchingEntries = entries.Where(e => e.Equals(expectedEntry));
            var errorMessage = string.Format("Did not find any entries that matched '{0}'", expectedEntry);
            if (!matchingEntries.Any()) throw new Exception(errorMessage);
        }

        public void Last_entry_should_equals(string expectedText)
        {
            GuardForNoEntries();
            var errorMessage = string.Format("Expected last entry to be '{0}', but was '{1}'", expectedText, entries.Last());
            if (!entries.Last().Equals(expectedText)) throw new Exception(errorMessage);
        }

        private void GuardForNoEntries()
        {
            if (entries.Count == 0) throw new Exception("No at all entries");
        }

        public void Last_entry_should_start_with(string expectedStartingText)
        {
            GuardForNoEntries();
            var errorMessage = string.Format("Expected last entry to begins with '{0}', but was '{1}'", expectedStartingText, entries.Last());
            if (!entries.Last().StartsWith(expectedStartingText)) throw new Exception(errorMessage);
        }
    }
}
