using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    public class Checkout
    {
        public int Scan(char Sku)
        {
            var priceList = new Dictionary<char, int>() {{'A', 50}, {'B', 30}};
            return priceList[Sku];
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
        [TestMethod]
        public void When_Scanning_2_A_Sku_Then_Total_Is_100()
        {
            var runningTotal = _checkout.Scan('A');
            runningTotal = _checkout.Scan('A');
            Assert.AreEqual(100, runningTotal);
        }
    }
}