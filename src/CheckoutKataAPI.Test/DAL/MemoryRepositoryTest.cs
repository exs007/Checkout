using System;
using System.Linq;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Test.DAL;
using Xunit;
using GenFu;
using System.Collections.Generic;

namespace CheckoutKataAPI.Test
{
    public class MemoryRepositoryTest
    {
        private readonly IRepository<FakeDataEntity> _repository;

        public MemoryRepositoryTest()
        {
            _repository = new MemoryRepository<FakeDataEntity>();
            A.Configure<FakeDataEntity>().Fill(p => p.Id, 0);
        }

        [Fact]
        public void AddEntityToRepositoryAndGenerateId()
        {
            var item = A.New<FakeDataEntity>();
            item = _repository.Add(item);

            Assert.NotEqual(0, item.Id);
        }

        [Fact]
        public void SelectExistEntityFromRepository()
        {
            var item = A.New<FakeDataEntity>();
            item = _repository.Add(item);

            var exist = _repository.Select(item.Id);
            Assert.NotNull(exist);
        }

        [Fact]
        public void SelectNullForNoExistEntityFromRepository()
        {
            var id = (new Random()).Next(1000000, 2000000);

            var notExist = _repository.Select(id);
            Assert.Null(notExist);
        }

        [Fact]
        public void SelectMultipleExistEntitiesFromRepository()
        {
            _repository.DeleteAll();

            var item1 = A.New<FakeDataEntity>();
            item1 = _repository.Add(item1);

            var item2 = A.New<FakeDataEntity>();
            item2 = _repository.Add(item2);

            var existItems = _repository.SelectAll();
            var existIds = existItems.Select(p => p.Id).ToList();
            Assert.True(existIds.Contains(item1.Id) && existIds.Contains(item2.Id));
        }

        [Fact]
        public void UpdateExistEntityInRepository()
        {
            var item = A.New<FakeDataEntity>();
            item = _repository.Add(item);

            item.StringData = "NewStringData";
            item.IntData += item.IntData;
            var updated = _repository.Update(item);
            Assert.True(updated);

            var exist = _repository.Select(item.Id);
            Assert.True(exist.StringData == item.StringData && exist.IntData == item.IntData);
        }

        [Fact]
        public void UpdateExistEntityInRepositoryAndCheckId()
        {
            var item = A.New<FakeDataEntity>();
            item = _repository.Add(item);

            var updated = _repository.Update(item);
            var exist = _repository.Select(item.Id);

            Assert.Equal(item.Id, exist.Id);
        }

        [Fact]
        public void DeleteNotExistEntityFromRepository()
        {
            var id = (new Random()).Next(1000000, 2000000);

            var deleted = _repository.Delete(id);
            Assert.True(!deleted);
        }

        [Fact]
        public void DeleteExistEntityFromRepository()
        {
            var item = A.New<FakeDataEntity>();
            item = _repository.Add(item);

            var deleted = _repository.Delete(item.Id);
            Assert.True(deleted);

            var notExist = _repository.Select(item.Id);
            Assert.Null(notExist);
        }

        [Fact]
        public void DeleteAllExistEntitiesFromRepository()
        {
            var item1 = A.New<FakeDataEntity>();
            item1 = _repository.Add(item1);

            var item2 = A.New<FakeDataEntity>();
            item2 = _repository.Add(item2);

            _repository.DeleteAll();
            var existItems = _repository.SelectAll();
            Assert.Equal(0, existItems.Count);
        }

        [Fact]
        public void SelectMultipleEntitiesFromRepository()
        {
            var item1 = A.New<FakeDataEntity>();
            item1 = _repository.Add(item1);
            var item2 = A.New<FakeDataEntity>();
            item2.StringData = item1.StringData;
            item2 = _repository.Add(item2);
            var ids = new List<int>(){ item1.Id, item2.Id };

            var existItems = _repository.Select(p => p.StringData == item2.StringData);
            Assert.Equal(ids.Count, existItems.Count);
            Assert.All(existItems, p => ids.Contains(p.Id));
        }

        [Fact]
        public void SelectZeroEntitiesFromRepository()
        {
            _repository.DeleteAll();

            var existItems = _repository.Select(p => p.StringData == "Test");
            Assert.Equal(0, existItems.Count);
        }
    }
}
