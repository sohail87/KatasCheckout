using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    public class Checkout
    {
        public int Scan(char Sku)
        {
            return 50;
        }
    }

    [TestClass]
    public class SpecTests
    {
        private readonly Checkout _checkout = new Checkout();

        [TestMethod]
        public void When_Scanning_1_A_Sku_Then_Total_Is_50()
        {
            Assert.AreEqual(50, _checkout.Scan('A'));
        }
    }
}
