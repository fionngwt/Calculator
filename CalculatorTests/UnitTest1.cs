using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;

namespace CalculatorTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void TestCalculator()
        {
            Assert.AreEqual(2, Program.Calculate("1 + 1"), "1 + 1");
            Assert.AreEqual(3, Program.Calculate("6 / 2"), "6 / 2");
            Assert.AreEqual(34.1, Program.Calculate("11.1 + 23"), "11.1 + 23");
            Assert.AreEqual(4, Program.Calculate("1 + 1 * 3"), "1 + 1 * 3");
            Assert.AreEqual(37, Program.Calculate("( 11.5 + 15.4 ) + 10.1"), "( 11.5 + 15.4 ) + 10.1");
            Assert.AreEqual(0.5, Program.Calculate("( 1 / 2 ) - 1 + 1"), "( 1 / 2 ) - 1 + 1");
            Assert.AreEqual(2, Program.Calculate("10 - ( 2 + 3 * ( 7 - 5 ) )"), "10 - ( 2 + 3 * ( 7 - 5 ) )");

            Assert.AreEqual(4850, Program.Calculate("100 * ( 99 - 2 ) / 2"), "100 * ( 99 - 2 ) / 2");
        }
    }
}
