using System;

namespace Elevator.Lib.DTO
{
    public class LevelDTO
    {
        public Guid Id { get; set; }
        public int LevelNumber { get; set; }
        public string Description { get; set; }
    }
}
