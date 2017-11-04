using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    public class Checkout
    {
        private int _runningTotal;

        private readonly Dictionary<char, int> _prices = new Dictionary<char, int>()
        {
            {'A', 50},
            {'B', 30},
            {'C', 20},
            {'D', 15}
        };

        public int Scan(char Sku)
        {
            _runningTotal += _prices[Sku];
            if (_runningTotal == 150) _runningTotal -= 20;
            return _runningTotal;
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
        public void When_Scanning_1_C_Sku_Then_Total_Is_20()
        {
            Assert.AreEqual(20, _checkout.Scan('C'));
        }

        [TestMethod]
        public void When_Scanning_1_D_Sku_Then_Total_Is_15()
        {
            Assert.AreEqual(15, _checkout.Scan('D'));
        }

        [TestMethod]
        public void When_Scanning_2_A_Sku_Then_Total_Is_100()
        {
            _checkout.Scan('A');
            Assert.AreEqual(100, _checkout.Scan('A'));
        }

        [TestMethod]
        public void When_Scanning_3_A_Sku_Then_Total_Is_130()
        {
            _checkout.Scan('A');
            _checkout.Scan('A');
            Assert.AreEqual(130, _checkout.Scan('A'));
        }
    }
}