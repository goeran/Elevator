using System;
using System.Linq;
using Elevator.Lib.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Elevator.Lib
{
    public abstract class MongoDbLevelDataStorage : ILevelDataStorage
    {
        private MongoServer server;
        private MongoDatabase database;
        private MongoCollection<LevelDTO> elevatorCollection;
        private const string SpecialCollectionNameForElevator = "__Elevator.Levels";
        private bool isInitialized;

        public void Initialize()
        {
            isInitialized = true;
            server = MongoServer.Create(ServerUrl);
            database = server.GetDatabase(DatabaseName);

            CreateElevatorCollectionIfItDoesntExist();
            elevatorCollection = database.GetCollection<LevelDTO>(SpecialCollectionNameForElevator);
        }

        private void CreateElevatorCollectionIfItDoesntExist()
        {
            GuardForInitialized();
            if (!database.CollectionExists(SpecialCollectionNameForElevator))
            {
                database.CreateCollection(SpecialCollectionNameForElevator);
            }
        }

        private void GuardForInitialized()
        {
            if (!isInitialized)
                throw new InvalidOperationException("Initialize must be called before any method on this object, to setup dependencies");
        }

        public bool HasStoredLevelInfo()
        {
            GuardForInitialized();
            return elevatorCollection.Count() > 0;
        }

        public void SaveCurrentLevel(Level level)
        {
            GuardForInitialized();
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
            GuardForInitialized();
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
