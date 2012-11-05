using System;
using System.Linq;
using Elevator.Lib.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Elevator.Lib
{
    public abstract class MongoDbLevelDataStorage : ILevelDataStorage
    {
        private readonly MongoServer server;
        private readonly MongoDatabase database;
        private readonly MongoCollection<LevelDTO> elevatorCollection;
        private const string SpecialCollectionNameForElevator = "__Elevator.Levels";

        public MongoDbLevelDataStorage()
        {
            server = MongoServer.Create(ServerUrl);
            database = server.GetDatabase(DatabaseName);

            CreateElevatorCollectionIfItDoesntExist();
            elevatorCollection = database.GetCollection<LevelDTO>(SpecialCollectionNameForElevator);
        }

        private void CreateElevatorCollectionIfItDoesntExist()
        {
            if (!database.CollectionExists(SpecialCollectionNameForElevator))
            {
                database.CreateCollection(SpecialCollectionNameForElevator);
            }
        }

        public bool HasStoredLevelInfo()
        {
            return elevatorCollection.Count() > 0;
        }

        public void SaveCurrentLevel(Level level)
        {
            var levelDto = new LevelDTO()
            {
                LevelNumber = level.Number,
                Description = level.Description
            };

            var existingLevelsWithSameNumber = elevatorCollection.Find(Query.EQ("LevelNumber", level.Number));
            if (existingLevelsWithSameNumber.Any())
            {
                var existingLevel = existingLevelsWithSameNumber.First();
                existingLevel.Description = level.Description;
                elevatorCollection.Save(existingLevel);
            }
            else
            {
                elevatorCollection.Save(levelDto);
            }
        }

        public Level GetCurrentLevel()
        {
            if (!HasStoredLevelInfo())
                throw new InvalidOperationException("No Level info is stored");

            var levelsOrderByLevelNumber = elevatorCollection.FindAll().SetSortOrder(SortBy.Descending("LevelNumber"));
            var firstLevel = levelsOrderByLevelNumber.First();
            
            return new Level(firstLevel.LevelNumber, firstLevel.Description);
        }

        public abstract string ServerUrl { get; }
        public abstract string DatabaseName { get; }
    }
}
