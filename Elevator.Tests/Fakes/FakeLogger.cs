using System;
using System.Collections.Generic;
using System.Linq;
using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class FakeLogger : ILogger
    {
        public FakeLogger()
        {
            Entries = new List<string>();
        }

        public void Log(string message)
        {
            Entries.Add(message);
        }

        public List<string> Entries { get; private set; }

        public string LastEntry()
        {
            if (Entries.Count == 0) return "";
            return Entries.Last();
        }

        public string LastEntry(int numberOfEntriesBack)
        {
            var delta = Entries.Count - numberOfEntriesBack;
            if (delta < 0) return "";
            return Entries[Entries.Count - (Math.Abs(numberOfEntriesBack) + 1)];
        }
    }
}
