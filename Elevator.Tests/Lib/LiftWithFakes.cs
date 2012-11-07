using Elevator.Lib;
using Elevator.Tests.Fakes;

namespace Elevator.Tests.Lib
{
    public class LifteWithFakes
    {
        public readonly Lift liftWithFakes;
        public readonly FakeLogger logger;
        public readonly FakeLevelDataStorage storage;

        public LifteWithFakes()
        {
            logger = new FakeLogger();
            storage = new FakeLevelDataStorage();
            liftWithFakes = new Lift(logger, storage);
        }
    }
}
