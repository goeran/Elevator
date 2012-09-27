using System.Collections.Generic;
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
            LastEntry = message;
            Entries.Add(message);
        }

        public string LastEntry { get; private set; }
        public List<string> Entries { get; private set; }
    }
}
