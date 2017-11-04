using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    public class Checkout
    {
        public int Scan(char Sku)
        {
            return Sku == 'A' ? 50 : 30;
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

        [TestMethod]
        public void When_Scanning_1_B_Sku_Then_Total_Is_30()
        {
            Assert.AreEqual(30, _checkout.Scan('B'));
        }
    }
}
