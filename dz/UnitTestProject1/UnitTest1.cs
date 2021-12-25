using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dz;


namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void loadItemFromFile_1855Result()
        {
            var customer = new Customer("1", "Abc", "890", false);
            var order = new Order(1, DateTime.Today, "", false, customer);
            var items = new ProductFromGetFile("item.dat").GetData;
            foreach (var item in items)
            {
                var orderLine = new OrderLine(item, 1);
                order.AddOrderLine(orderLine);
            }
            float expect = 1855;
            float actual = order.GetTotalCost();
            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void loadCustomersFromFile_1000Result()
        {
            var customers = new
           CustomerFromGetFile("customer.dat").GetData;
            var customer = customers[1];
            var order = new Order(1, DateTime.Today, "", false,
           customer);
            var item = new Item("1fs", "Яблоки", 10);
            var orderLine = new OrderLine(item, 100);
            order.AddOrderLine(orderLine);
            float expect = 1000;
            float actual = order.GetTotalCost();
            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void loadItemAndCustomerFromFile_2592_5Result()
        {
            var customers = new
           CustomerFromGetFile("customer.dat").GetData;
            var customer = customers[0];
            var order = new Order(1, DateTime.Today, "", false,
           customer);
            var items = new ProductFromGetFile("item.dat").GetData;
            var item = items[0];
            var orderLine = new OrderLine(item, 5);
            order.AddOrderLine(orderLine);
            item = items[1];
            orderLine = new OrderLine(item, 5);
            order.AddOrderLine(orderLine);
            float expect = 2592.5f;
            float actual = order.GetTotalCost();
            Assert.AreEqual(expect, actual);
        }
    }
}
