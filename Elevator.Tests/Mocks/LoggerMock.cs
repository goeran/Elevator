using System;
using System.Collections.Generic;
using System.Linq;
using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class LoggerMock : ILogger
    {
        public LoggerMock()
        {
            Entries = new List<string>();
        }

        public void Log(string message)
        {
            Entries.Add(message);
        }

        public List<string> Entries { get; private set; }

        public void Last_entry_should_equals(string expectedText)
        {
            GuardForNoEntries();
            var errorMessage = string.Format("Expected last entry to be '{0}', but was '{1}'", expectedText, Entries.Last());
            if (!Entries.Last().Equals(expectedText)) throw new Exception(errorMessage);
        }

        private void GuardForNoEntries()
        {
            if (Entries.Count == 0) throw new Exception("No at all entries");
        }

        public void Last_entry_should_start_with(string expectedStartingText)
        {
            GuardForNoEntries();
            var errorMessage = string.Format("Expected last entry to begins with '{0}', but was '{1}'", expectedStartingText, Entries.Last());
            if (!Entries.Last().StartsWith(expectedStartingText)) throw new Exception(errorMessage);
        }

        public string LastEntry(int numberOfEntriesBack)
        {
            var delta = Entries.Count - numberOfEntriesBack;
            if (delta < 0) return "";
            return Entries[Entries.Count - (Math.Abs(numberOfEntriesBack) + 1)];
        }
    }
}
