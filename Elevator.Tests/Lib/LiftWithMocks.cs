using Elevator.Lib;
using Elevator.Tests.Fakes;

namespace Elevator.Tests.Lib
{
    public class LiftWithMocks
    {
        public readonly Lift liftWithFakes;
        public readonly LoggerMock logger;
        public readonly LevelDataStorageMock storage;

        public LiftWithMocks()
        {
            logger = new LoggerMock();
            storage = new LevelDataStorageMock();
            liftWithFakes = new Lift(logger, storage);
        }
    }
}
