using System;

namespace Elevator.AcceptanceTests.Migrations
{
    public class ElevatorLevel1
    {
        public int Level = 1;
        public string Description = "First level";

        public ElevatorLevel1()
        {
        }

        public void Up()
        {
            Console.WriteLine("Arrived at first level!");
        }
    }
}
