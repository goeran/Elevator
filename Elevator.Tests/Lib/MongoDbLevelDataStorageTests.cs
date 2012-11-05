using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elevator.Lib;
using Elevator.Lib.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;

namespace Elevator.Tests.Lib
{
    class MongoDbLevelDataStorageTests
    {
        [TestFixture]
        public class When_saving_current_level : SharedSetupForMongoDb
        {
            [Test]
            public void It_should_store_level_info_in_a_unique_collection_for_Elevator()
            {
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(0, "Init level"));

                Assert.IsTrue(database.CollectionExists("__Elevator.Levels"), "Expected collection to be created");
            }

            [Test]
            public void It_should_create_a_new_document_for_the_level()
            {
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(0, "Init level"));

                var levelsWithLevelNumber0 = elevatorCollection.FindAs<LevelDTO>(Query.EQ("LevelNumber", 0));
                Assert.AreEqual(1, levelsWithLevelNumber0.Count(), "Expected document for the level to be created");
            }

            [Test]
            public void It_should_not_duplicate_documents_with_same_LevelNumber()
            {
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(0, "Init level"));
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(0, "Init level with updated text"));

                var levelsWithLevelNumber0 = elevatorCollection.FindAs<LevelDTO>(Query.EQ("LevelNumber", 0));
                Assert.AreEqual(1, levelsWithLevelNumber0.Count(), "Expected document for the level to be created");
                Assert.AreEqual("Init level with updated text", levelsWithLevelNumber0.First().Description);
            }
        }

        [TestFixture]
        public class When_checking_if_has_stored_level_info : SharedSetupForMongoDb
        {
            [Test]
            public void It_should_say_yes_when_level_data_is_stored()
            {
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(0, "init data level"));
                Assert.IsTrue(mongoDbLevelDataStorage.HasStoredLevelInfo(), "Expected to have stored level info");
            }

            [Test]
            public void It_should_say_no_when_no_level_data_is_stored()
            {
                Assert.IsFalse(mongoDbLevelDataStorage.HasStoredLevelInfo(), "Didn't expect stored level info");
            }
        }

        [TestFixture]
        public class When_getting_current_level : SharedSetupForMongoDb
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "No Level info is stored")]
            public void It_should_throw_exception_when_no_level_info_is_stored()
            {
                mongoDbLevelDataStorage.GetCurrentLevel();
            }

            [Test]
            public void It_should_store_the_level_with_the_largest_number()
            {
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(0, "Ground level"));
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(-1, "Basement"));
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(1, "First level"));
                mongoDbLevelDataStorage.SaveCurrentLevel(new Level(2, "Second level"));

                var currentLevel = mongoDbLevelDataStorage.GetCurrentLevel();

                Assert.AreEqual(2, currentLevel.Number);
                Assert.AreEqual("Second level", currentLevel.Description);
            }
        }


        internal class SharedSetupForMongoDb
        {
            protected CustomMongoDbLevelDataStorage mongoDbLevelDataStorage;
            protected MongoDatabase database;
            protected MongoCollection elevatorCollection;

            [SetUp]
            public void Setup()
            {
                mongoDbLevelDataStorage = new CustomMongoDbLevelDataStorage();
                mongoDbLevelDataStorage.Initialize();

                var mongoServer = MongoServer.Create(mongoDbLevelDataStorage.ServerUrl);
                database = mongoServer.GetDatabase(mongoDbLevelDataStorage.DatabaseName);
                elevatorCollection = database.GetCollection<LevelDTO>("__Elevator.Levels");
            }

            [TearDown]
            public void Teardown()
            {
                elevatorCollection.RemoveAll();
            }
        }

        public class CustomMongoDbLevelDataStorage : MongoDbLevelDataStorage
        {
            public override string ServerUrl
            {
                get { return "mongodb://localhost"; }
            }

            public override string DatabaseName
            {
                get { return "ElevatorAutomatedTests"; }
            }
        }
    }
}
