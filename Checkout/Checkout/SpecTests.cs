using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    public class Checkout
    {
        public Checkout(Dictionary<char, Money> prices, List<DiscountRule> discountRules)
        {
            _prices = prices;
            _discountRules = discountRules;
        }

        private Money _runningTotal;

        private readonly Dictionary<char, Money> _prices;

        private readonly List<DiscountRule> _discountRules;

        private readonly List<char> _basket = new List<char>();

        public void Scan(char Sku)
        {
            _basket.Add(Sku);
        }

        public Money GetTotal()
        {
            _runningTotal = new Money(0);
            SumBasketItems();
            ApplyDiscount();
            return _runningTotal;
        }

        private void SumBasketItems()
        {
            _basket.ForEach(sku => _runningTotal = _runningTotal.Add(_prices[sku]));
        }

        private void ApplyDiscount()
        {
            _discountRules.ForEach(d => _runningTotal = _runningTotal.Add(d.GetTotalDiscountFor(_basket)));
        }
    }

    public class DiscountRule
    {
        public char Sku { get; }
        public int Quantity { get; }
        public Money Amount { get; }

        public DiscountRule(char sku, int quantity, Money amount)
        {
            Sku = sku;
            Quantity = quantity;
            Amount = amount;
        }

        public Money GetTotalDiscountFor(List<char> basket)
        {
            var skuCount = basket.Count(sku => sku.Equals(Sku));
            var instances = skuCount / Quantity;
            return Amount.MultiplyBy(instances);
        }
    }
    public class Money
    {
        private readonly int _value;

        public Money(int value)
        {
            _value = value;
        }

        public Money Add(Money money)
        {
            return new Money(_value + money._value);
        }

        public override bool Equals(object obj)
        {
            var money = (Money) obj;
                return money != null && money._value.Equals(this._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public Money MultiplyBy(int instances)
        {
            return new Money(instances * _value);
        }
    }
    [TestClass]
    public class SpecTests
    {
        private Checkout _checkout;

        [TestInitialize]
        public void TestSetup()
        {
            var prices = new Dictionary<char, Money>()
            {
                {'A', new Money(50)},
                {'B', new Money(30)},
                {'C', new Money(20)},
                {'D', new Money(15)}
            };

            var discountRules = new List<DiscountRule>()
            {
                new DiscountRule('A', 3, -20),
                new DiscountRule('B', 2, -15)
            };

            _checkout = new Checkout(prices, discountRules);
        }


        [TestMethod]
        public void When_Scanning_1_A_Sku_Then_Total_Is_50()
        {
            _checkout.Scan('A');
            Assert.AreEqual(new Money(50), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_B_Sku_Then_Total_Is_30()
        {
            _checkout.Scan('B');
            Assert.AreEqual(new Money(30), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_C_Sku_Then_Total_Is_20()
        {
            _checkout.Scan('C');
            Assert.AreEqual(new Money(20), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_D_Sku_Then_Total_Is_15()
        {
            _checkout.Scan('D');
            Assert.AreEqual(new Money(15), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_2_A_Sku_Then_Total_Is_100()
        {
            _checkout.Scan('A');
            _checkout.Scan('A');
            Assert.AreEqual(new Money(100), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_3_A_Sku_Then_Total_Is_130()
        {
            _checkout.Scan('A');
            _checkout.Scan('A');
            _checkout.Scan('A');
            Assert.AreEqual(new Money(130), _checkout.GetTotal());
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
            Assert.AreEqual(new Money(260), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_2_B_Sku_Then_Total_Is_45()
        {
            _checkout.Scan('B');
            _checkout.Scan('B');
            Assert.AreEqual(new Money(45), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_4_B_Sku_Then_Total_Is_90()
        {
            _checkout.Scan('B');
            _checkout.Scan('B');
            _checkout.Scan('B');
            _checkout.Scan('B');
            Assert.AreEqual(new Money(90), _checkout.GetTotal());
        }
        [TestMethod]
        public void When_Scanning_B_A_B_Sku_Then_Total_Is_95()
        {
            _checkout.Scan('B');
            _checkout.Scan('A');
            _checkout.Scan('B');
            Assert.AreEqual(new Money(95), _checkout.GetTotal());
        }
        [TestMethod]
        public void When_Scanning_Nothing_Then_Total_Is_0()
        {
            Assert.AreEqual(new Money(0), _checkout.GetTotal());
        }
        [TestMethod]
        public void When_Getting_Total_Twice_Then_Total_Remains_The_Same()
        {
            _checkout.Scan('A');
            Assert.AreEqual(new Money(50), _checkout.GetTotal());
            Assert.AreEqual(new Money(50), _checkout.GetTotal());
        }
    }


}