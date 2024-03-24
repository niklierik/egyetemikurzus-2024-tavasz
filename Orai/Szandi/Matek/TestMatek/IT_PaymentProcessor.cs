using Matek;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMatek
{
    [TestFixture]
    public class IT_PaymentProcessor
    {
        PaymentProcessor _sut;
        Mock<IPaymentGateway> _mockGateway;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IPaymentGateway>();
            _mockGateway.Setup(x => x.ProcessPayment(It.IsAny<double>())).Returns(true);
            _sut = new PaymentProcessor(_mockGateway.Object);
        }

        [Test]
        public void Test_ProcessPayment()
        {
            //Arrange
            double amount = 100;
            //Act
            var actual = _sut.ProcessPayment(amount);
            //Assert
            _mockGateway.Verify(x => x.ProcessPayment(amount), Times.Once);
        }
    }
}
