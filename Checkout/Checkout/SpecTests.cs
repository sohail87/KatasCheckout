using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<char> _basket = new List<char>();

        public void Scan(char Sku)
        {
            _basket.Add(Sku);

        }

        public int GetTotal()
        {
            _basket.ForEach(sku => _runningTotal += _prices[sku]);
            var aCount = _basket.Count(sku => sku.Equals('A'));
            var discountA3instances = aCount / 3;
            _runningTotal -= 20 * discountA3instances;
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
            _checkout.Scan('A');
            Assert.AreEqual(50, _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_B_Sku_Then_Total_Is_30()
        {
            _checkout.Scan('B');
            Assert.AreEqual(30, _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_C_Sku_Then_Total_Is_20()
        {
            _checkout.Scan('C');
            Assert.AreEqual(20, _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_D_Sku_Then_Total_Is_15()
        {
            _checkout.Scan('D');
            Assert.AreEqual(15, _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_2_A_Sku_Then_Total_Is_100()
        {
            _checkout.Scan('A');
            _checkout.Scan('A');
            Assert.AreEqual(100, _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_3_A_Sku_Then_Total_Is_130()
        {
            _checkout.Scan('A');
            _checkout.Scan('A');
            _checkout.Scan('A');
            Assert.AreEqual(130, _checkout.GetTotal());
        }
        [TestMethod]
        public void When_Scanning_6_A_Sku_Then_Total_Is_260()
        {
            _checkout.Scan('A');
            _checkout.Scan('A');
            _checkout.Scan('A');
            _checkout.Scan('A');
            _checkout.Scan('A');
            _checkout.Scan('A');
            Assert.AreEqual(260, _checkout.GetTotal());
        }
    }
}