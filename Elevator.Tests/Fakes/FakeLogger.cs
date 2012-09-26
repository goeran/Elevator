using Elevator.Lib;

namespace Elevator.Tests.Fakes
{
    public class FakeLogger : ILogger
    {
        public void Log(string message)
        {
            LastEntry = message;
        }

        public string LastEntry { get; private set; }
    }
}
