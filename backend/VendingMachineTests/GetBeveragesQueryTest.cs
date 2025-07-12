using AutoFixture;
using backend.Application;
using backend.Domain;
using backend.Repository;
using Moq;
using NUnit.Framework;

namespace VendingMachineTests
{
    [TestFixture]
    public class GetBeveragesQueryTest
    {
        private Mock<IVendingMachineRepository> _mockRepository;
        private GetBeveragesQuery _query;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockRepository = new Mock<IVendingMachineRepository>();
            _query = new GetBeveragesQuery(_mockRepository.Object);
        }

        [Test]
        public void AsConsumer_WhenViewingAvailableBeverages_ShouldReturnProductsFromRepository()
        {
            var expectedProducts = _fixture.CreateMany<BeverageModel>(4).Cast<IProductModel>();
            _mockRepository.Setup(r => r.GetProducts()).Returns(expectedProducts);

            var result = _query.Execute();

            Assert.That(result, Is.EqualTo(expectedProducts));
            _mockRepository.Verify(r => r.GetProducts(), Times.Once);
        }

        [Test]
        public void AsConsumer_WhenViewingAvailableBeverages_ShouldShowInitialInventoryWithCorrectPrices()
        {
            var initialBeverages = new List<BeverageModel>
            {
                _fixture.Build<BeverageModel>()
                    .With(b => b.Name, "Coca Cola")
                    .With(b => b.Price, 800m)
                    .With(b => b.Quantity, 10)
                    .Create(),
                _fixture.Build<BeverageModel>()
                    .With(b => b.Name, "Pepsi")
                    .With(b => b.Price, 750m)
                    .With(b => b.Quantity, 8)
                    .Create()
            }.Cast<IProductModel>();

            _mockRepository.Setup(r => r.GetProducts()).Returns(initialBeverages);

            var result = _query.Execute();

            var beveragesList = result.Cast<BeverageModel>().ToList();
            Assert.That(beveragesList.Any(b => b.Name == "Coca Cola" && b.Price == 800m && b.Quantity == 10), Is.True);
            Assert.That(beveragesList.Any(b => b.Name == "Pepsi" && b.Price == 750m && b.Quantity == 8), Is.True);
        }

        [Test]
        public void AsConsumer_WhenNoProductsAvailable_ShouldReturnEmptyList()
        {
            _mockRepository.Setup(r => r.GetProducts()).Returns(Enumerable.Empty<IProductModel>());

            var result = _query.Execute();

            Assert.That(result, Is.Empty);
        }
    }
}