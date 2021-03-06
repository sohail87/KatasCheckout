﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout
{
    public class Checkout
    {
        public Checkout(Dictionary<Product, Money> prices, List<DiscountRule> discountRules)
        {
            _prices = prices;
            _discountRules = discountRules;
        }

        private Money _runningTotal;

        private readonly Dictionary<Product, Money> _prices;

        private readonly List<DiscountRule> _discountRules;

        private readonly List<Product> _basket = new List<Product>();

        public void Scan(Product Sku)
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
        public DiscountRule(Product sku, int quantity, Money amount)
        {
            Sku = sku;
            Quantity = quantity;
            Amount = amount;
        }

        public Product Sku { get; }
        public int Quantity { get; }
        public Money Amount { get; }

        public Money GetTotalDiscountFor(List<Product> basket)
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

        public Money MultiplyBy(int instances)
        {
            return new Money(instances * _value);
        }

        public override bool Equals(object obj)
        {
            var moneyToCompare = (Money) obj;
            return moneyToCompare != null && this._value.Equals(moneyToCompare._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class Product
    {
        private readonly char _name;

        public Product(char name)
        {
            _name = name;
        }

        public override bool Equals(object obj)
        {
            var product = (Product)obj;
            return product != null && product._name == _name;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }

    [TestClass]
    public class SpecTests
    {
        private Checkout _checkout;

        [TestInitialize]
        public void TestSetup()
        {
            var prices = new Dictionary<Product, Money>()
            {
                {new Product('A'), new Money(50)},
                {new Product('B'), new Money(30)},
                {new Product('C'), new Money(20)},
                {new Product('D'), new Money(15)}
            };

            var discountRules = new List<DiscountRule>()
            {
                new DiscountRule(new Product('A'), 3, new Money(-20)),
                new DiscountRule(new Product('B'), 2, new Money(-15))
            };

            _checkout = new Checkout(prices, discountRules);
        }


        [TestMethod]
        public void When_Scanning_1_A_Sku_Then_Total_Is_50()
        {
            _checkout.Scan(new Product('A'));
            Assert.AreEqual(new Money(50), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_B_Sku_Then_Total_Is_30()
        {
            _checkout.Scan(new Product('B'));
            Assert.AreEqual(new Money(30), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_C_Sku_Then_Total_Is_20()
        {
            _checkout.Scan(new Product('C'));
            Assert.AreEqual(new Money(20), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_1_D_Sku_Then_Total_Is_15()
        {
            _checkout.Scan(new Product('D'));
            Assert.AreEqual(new Money(15), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_2_A_Sku_Then_Total_Is_100()
        {
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            Assert.AreEqual(new Money(100), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_3_A_Sku_Then_Total_Is_130()
        {
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            Assert.AreEqual(new Money(130), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_6_A_Sku_Then_Total_Is_260()
        {
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('A'));
            Assert.AreEqual(new Money(260), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_2_B_Sku_Then_Total_Is_45()
        {
            _checkout.Scan(new Product('B'));
            _checkout.Scan(new Product('B'));
            Assert.AreEqual(new Money(45), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_4_B_Sku_Then_Total_Is_90()
        {
            _checkout.Scan(new Product('B'));
            _checkout.Scan(new Product('B'));
            _checkout.Scan(new Product('B'));
            _checkout.Scan(new Product('B'));
            Assert.AreEqual(new Money(90), _checkout.GetTotal());
        }

        [TestMethod]
        public void When_Scanning_B_A_B_Sku_Then_Total_Is_95()
        {
            _checkout.Scan(new Product('B'));
            _checkout.Scan(new Product('A'));
            _checkout.Scan(new Product('B'));
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
            _checkout.Scan(new Product('A'));
            Assert.AreEqual(new Money(50), _checkout.GetTotal());
            Assert.AreEqual(new Money(50), _checkout.GetTotal());
        }
    }
}