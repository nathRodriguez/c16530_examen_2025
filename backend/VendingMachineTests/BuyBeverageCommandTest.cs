using AutoFixture;
using backend.Application;
using backend.Application.DTOs;
using backend.Domain;
using backend.Repository;
using Moq;
using NUnit.Framework;

namespace VendingMachineTests
{
    [TestFixture]
    public class BuyBeverageCommandTest
    {
        private Mock<IVendingMachineRepository> _mockRepository;
        private BuyBeverageCommand _command;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockRepository = new Mock<IVendingMachineRepository>();
            _command = new BuyBeverageCommand(_mockRepository.Object);
        }

        [Test]
        public void AsConsumer_WhenMakingSuccessfulPurchase_ShouldReturnSuccessResponse()
        {
            var buyRequest = _fixture.Build<BuyProducstRequestModel>()
                .With(r => r.Products, new List<ProductInformation>
                {
                    new ProductInformation { Name = "Coca Cola", Quantity = 2 }
                })
                .With(r => r.Payment, new List<MoneyInformation>
                {
                    new MoneyInformation { Value = 1000, Quantity = 2 }
                })
                .Create();

            var expectedResponse = _fixture.Build<BuyProductsResponseDto>()
                .With(r => r.Status, "success")
                .With(r => r.TotalCost, 1600)
                .With(r => r.TotalPayment, 2000)
                .With(r => r.ChangeAmount, 400)
                .Create();

            _mockRepository.Setup(r => r.ProcessPurchase(buyRequest)).Returns(expectedResponse);

            var result = _command.Execute(buyRequest);

            Assert.That(result, Is.EqualTo(expectedResponse));
            Assert.That(result.Status, Is.EqualTo("success"));
            _mockRepository.Verify(r => r.ProcessPurchase(buyRequest), Times.Once);
        }

        [Test]
        public void AsConsumer_WhenInsufficientFunds_ShouldReturnErrorResponse()
        {
            var buyRequest = _fixture.Build<BuyProducstRequestModel>()
                .With(r => r.Products, new List<ProductInformation>
                {
                    new ProductInformation { Name = "Coca Cola", Quantity = 1 }
                })
                .With(r => r.Payment, new List<MoneyInformation>
                {
                    new MoneyInformation { Value = 500, Quantity = 1 }
                })
                .Create();

            var expectedResponse = _fixture.Build<BuyProductsResponseDto>()
                .With(r => r.Status, "error")
                .With(r => r.Message, "Dinero insuficiente para realizar la compra.")
                .Create();

            _mockRepository.Setup(r => r.ProcessPurchase(buyRequest)).Returns(expectedResponse);

            var result = _command.Execute(buyRequest);

            Assert.That(result, Is.EqualTo(expectedResponse));
            Assert.That(result.Status, Is.EqualTo("error"));
            Assert.That(result.Message, Is.EqualTo("Dinero insuficiente para realizar la compra."));
        }

        [Test]
        public void AsConsumer_WhenRequestingMoreThanAvailable_ShouldReturnErrorResponse()
        {
            var buyRequest = _fixture.Build<BuyProducstRequestModel>()
                .With(r => r.Products, new List<ProductInformation>
                {
                    new ProductInformation { Name = "Coca Cola", Quantity = 15 }
                })
                .Create();

            var expectedResponse = _fixture.Build<BuyProductsResponseDto>()
                .With(r => r.Status, "error")
                .With(r => r.Message, "No hay suficientes unidades de: Coca Cola")
                .Create();

            _mockRepository.Setup(r => r.ProcessPurchase(buyRequest)).Returns(expectedResponse);

            var result = _command.Execute(buyRequest);

            Assert.That(result.Status, Is.EqualTo("error"));
            Assert.That(result.Message, Does.Contain("No hay suficientes unidades"));
        }

        [Test]
        public void AsConsumer_WhenMachineCannotProvideChange_ShouldReturnFailureResponse()
        {
            var buyRequest = _fixture.Create<BuyProducstRequestModel>();

            var expectedResponse = _fixture.Build<BuyProductsResponseDto>()
                .With(r => r.Status, "error")
                .With(r => r.Message, "Fallo al realizar la compra")
                .Create();

            _mockRepository.Setup(r => r.ProcessPurchase(buyRequest)).Returns(expectedResponse);

            var result = _command.Execute(buyRequest);

            Assert.That(result.Status, Is.EqualTo("error"));
            Assert.That(result.Message, Is.EqualTo("Fallo al realizar la compra"));
        }

        [Test]
        public void AsConsumer_WhenMachineIsOutOfService_ShouldReturnOutOfServiceResponse()
        {
            var buyRequest = _fixture.Create<BuyProducstRequestModel>();

            var expectedResponse = _fixture.Build<BuyProductsResponseDto>()
                .With(r => r.Status, "out_of_service")
                .With(r => r.Message, "Fuera de servicio")
                .Create();

            _mockRepository.Setup(r => r.ProcessPurchase(buyRequest)).Returns(expectedResponse);

            var result = _command.Execute(buyRequest);

            Assert.That(result.Status, Is.EqualTo("out_of_service"));
            Assert.That(result.Message, Is.EqualTo("Fuera de servicio"));
        }

        [Test]
        public void AsConsumer_WhenReceivingChange_ShouldShowCorrectChangeBreakdown()
        {
            var buyRequest = _fixture.Create<BuyProducstRequestModel>();

            var expectedResponse = _fixture.Build<BuyProductsResponseDto>()
                .With(r => r.Status, "success")
                .With(r => r.ChangeAmount, 650)
                .With(r => r.ChangeBreakdown, new List<ChangeBreakdownDto>
                {
                    new ChangeBreakdownDto { Value = 500, Quantity = 1 },
                    new ChangeBreakdownDto { Value = 100, Quantity = 1 },
                    new ChangeBreakdownDto { Value = 50, Quantity = 1 }
                })
                .Create();

            _mockRepository.Setup(r => r.ProcessPurchase(buyRequest)).Returns(expectedResponse);

            var result = _command.Execute(buyRequest);

            Assert.That(result.ChangeAmount, Is.EqualTo(650));
            Assert.That(result.ChangeBreakdown.Count, Is.EqualTo(3));
            Assert.That(result.ChangeBreakdown.Any(c => c.Value == 500 && c.Quantity == 1), Is.True);
            Assert.That(result.ChangeBreakdown.Any(c => c.Value == 100 && c.Quantity == 1), Is.True);
            Assert.That(result.ChangeBreakdown.Any(c => c.Value == 50 && c.Quantity == 1), Is.True);
        }
    }
}