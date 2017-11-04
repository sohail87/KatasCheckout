using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    [TestClass]
    public class SpecTests
    {
        [TestMethod]
        public void When_Scanning_1_A_Sku_Then_Total_Is_50()
        {
            Assert.AreEqual(50, Scan('A'));
        }

        private int Scan(char Sku)
        {
            return 50;
        }
    }
}
