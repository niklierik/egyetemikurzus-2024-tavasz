using Matek;

namespace TestMatek
{
    [TestFixture]
    public class UT_FibonacciCalculator
    {
        FibonacciCalculator _sut;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EnsureThat_Fibo_Throws_If_N_Less_Zero()
        {
            //Arrange
            _sut = new FibonacciCalculator();
            
            //Assert
            Assert.Throws<ArgumentException>(() => _sut.CalculateFibo(-1));
        }

        [Test]
        public void EnsureThat_Fibo_Works_Correct_One()
        {
            //Arrange
            _sut = new FibonacciCalculator();
            //Act
            var actual = _sut.CalculateFibo(1);
            //Assert

    //Assert.Equals(0, actual[0]);
            Assert.AreEqual(0, actual[0]);
        }


        [Test]
        public void EnsureThat_Fibo_Works_Correct_Two()
        {
            //Arrange
            _sut = new FibonacciCalculator();
            //Act
            var actual = _sut.CalculateFibo(2);
            //Assert

            //Assert.Equals(0, actual[0]);
            Assert.AreEqual(1, actual[1]);
        }


    }
}