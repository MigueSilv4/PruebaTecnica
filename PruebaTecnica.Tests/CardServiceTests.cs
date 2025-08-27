using Microsoft.VisualStudio.TestTools.UnitTesting;
using PruebaTecnica.Models;
using PruebaTecnica.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Tests
{
    [TestClass]
    public class CardServiceTests
    {
        private SqliteInMemoryFactory _factory = null!;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new SqliteInMemoryFactory();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _factory.Dispose();
        }

        [TestMethod]
        public async Task Create_ShouldSaveCard_WhenValidCard()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            var card = new Card
            {
                CustomerName = "Juan",
                CardNumber = "123456",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = await service.Create(card);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("Juan", result.Data.CustomerName);
        }

        [TestMethod]
        public async Task Create_ShouldFail_WhenCardNumberAlreadyExists()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            var card1 = new Card { CustomerName = "Ana", CardNumber = "DUP123", CreatedAt = DateTime.UtcNow };
            var card2 = new Card { CustomerName = "Luis", CardNumber = "DUP123", CreatedAt = DateTime.UtcNow };

            await service.Create(card1);

            // Act
            var result = await service.Create(card2);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("El número de tarjeta ya existe.", result.Message);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnCard_WhenExists()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            var card = new Card { CustomerName = "Carlos", CardNumber = "CAR123", CreatedAt = DateTime.UtcNow };
            var created = await service.Create(card);

            // Act
            var result = await service.GetById(created.Data.Id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Carlos", result.Data.CustomerName);
        }

        [TestMethod]
        public async Task GetById_ShouldFail_WhenCardDoesNotExist()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            // Act
            var result = await service.GetById(999);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("No se encontró la tarjeta con el ID especificado.", result.Message);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnPaginatedCards_WhenCardsExist()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            for (int i = 1; i <= 15; i++)
            {
                await service.Create(new Card
                {
                    CustomerName = $"Cliente {i}",
                    CardNumber = $"CARD{i:000}",
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Act
            var result = await service.GetAll(pageIndex: 2, pageSize: 5);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(5, result.Data.Items.Count);
            Assert.AreEqual(15, result.Data.TotalCount);
            Assert.AreEqual("Cliente 6", result.Data.Items.First().CustomerName);
        }

        [TestMethod]
        public async Task GetAll_ShouldFilterByCustomerName_WhenFilterIsProvided()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            await service.Create(new Card { CustomerName = "Ana", CardNumber = "A001", CreatedAt = DateTime.UtcNow });
            await service.Create(new Card { CustomerName = "Bob", CardNumber = "B001", CreatedAt = DateTime.UtcNow });
            await service.Create(new Card { CustomerName = "Analia", CardNumber = "A002", CreatedAt = DateTime.UtcNow });

            // Act
            var result = await service.GetAll(customerName: "Ana");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(2, result.Data.Items.Count); // Ana + Analia
            Assert.IsTrue(result.Data.Items.All(c => c.CustomerName.Contains("Ana")));
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoCardsMatchFilter()
        {
            // Arrange
            using var context = _factory.CreateContext();
            var service = new CardService(context);

            await service.Create(new Card { CustomerName = "Charlie", CardNumber = "C001", CreatedAt = DateTime.UtcNow });

            // Act
            var result = await service.GetAll(customerName: "Inexistente");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Data.Items.Count);
        }
    }
}
