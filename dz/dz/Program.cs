using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace dz
{
    public class Order // заказы
    {
        const float EXPRESS_DELIVERY_MULTIPLIER = 1.25f;
        const float PRIVILEGE_DDISCOUNT = 0.85f;
        const int DISCOUNT_COST = 1500;
        public Order(int number, DateTime time, string address,
        bool expressDelivery, Customer customer)
        {
            Number = number;
            CreationDate = time;
            Address = address;
            ExpressDelivery = expressDelivery;
            Customer = customer;
        }
        public int Number { get; }
        public DateTime CreationDate { get; }
        public string Address { get; }
        public bool ExpressDelivery { get; }
        private List<OrderLine> _orderLines = new List<OrderLine>();
        public Customer Customer { get; }
        public float GetTotalCost()
        {
            float totalCost = 0;
            foreach (var orderLine in _orderLines)
            {
                totalCost += orderLine.Cost;
            }
            if (ExpressDelivery)
            {
                totalCost *= EXPRESS_DELIVERY_MULTIPLIER;
            }
            if ((totalCost > DISCOUNT_COST) &&
           (Customer.Privileged))
            {
                totalCost *= PRIVILEGE_DDISCOUNT;
            }
            return totalCost;
        }
        public void AddOrderLine(OrderLine orderLine)
        {
            _orderLines.Add(orderLine);
        }
        public void RemoveOrderLine(OrderLine orderLine)
        {
            _orderLines.Remove(orderLine);
        }
    }
    public class Customer //клиент
    {
        public Customer(string code, string fullName, string
       contactPhone, bool privileged)
        {
            Code = code;
            FullName = fullName;
            ContactPhone = contactPhone;
            Privileged = privileged;
        }

        public string Code { get; }
        public string FullName { get; }
        public string ContactPhone { get; }
        public bool Privileged { get; }
    }

    public class OrderLine
    {
        public OrderLine(Item item, float quatity)
        {
            Item = item;
            Quatity = quatity;
        }
        public Item Item { get; set; }
        public float Quatity { get; set; }
        public float Cost
        {
            get { return Item.UnitPrice * Quatity; }
        }
        public void ChangeQuantity(float quantity)
        {
            Quatity = quantity;
        }
    }
    public class Item
    {
        public Item(string article, string name, float unitPrice)
        {
            Article = article;
            Name = name;
            UnitPrice = unitPrice;
        }
        public string Article { get; }
        public string Name { get; }
        public float UnitPrice { get; }
    }
    interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetData { get; }
    }
    public class CustomerFromGetFile : IRepository<Customer>
    {
        private string _path;
        public CustomerFromGetFile(string Path)
        {
            _path = Path;
        }
        public List<Customer> GetData
        {
            get
            {
                var lines = File.ReadAllLines(_path);
                List<Customer> customers = new List<Customer>();
                var regex = new Regex(@"^(.*?)\|(.*?)\|(.*?)\|(N|Y)");
                foreach (var line in lines)
                {
                    if (regex.IsMatch(line))
                    {
                        var matches = regex.Matches(line);
                        if (matches[0].Groups.Count == 5)
                        {
                            var code = matches[0].Groups[1].Value;
                            var phoneNumber =
                            matches[0].Groups[2].Value;
                            var fullName =
                            matches[0].Groups[3].Value;
                            var privileged =
                            (matches[0].Groups[4].Value == "Y");
                            //Console.WriteLine($"{article} {name} { price}");
                            var customer = new Customer(code, fullName, phoneNumber, privileged);
                            customers.Add(customer);
                        }
                        else
                        {
                            throw new Exception("Line does not match pattern");
                        }
                    }
                }
                return customers;
            }
        }
    }
    public class ProductFromGetFile : IRepository<Item>
    {
        private string _path;
        public ProductFromGetFile(string Path)
        {
            _path = Path;
        }

        public List<Item> GetData
        {
            get
            {
                var lines = File.ReadAllLines(_path);
                List<Item> items = new List<Item>();
                var regex = new Regex(@"^(.*?)\|(.*?)\|(\d+\.\d{2})");
                foreach (var line in lines)
                {
                    if (regex.IsMatch(line))
                    {
                        var matches = regex.Matches(line);
                        if (matches[0].Groups.Count == 4)
                        {
                            var article =
                                matches[0].Groups[1].Value;
                            var name = matches[0].Groups[2].Value;
                            var price = (float)
                            double.Parse(matches[0].Groups[3].Value.Replace('.', ','));
                            //Console.WriteLine($"{article} {name} { price}");
                            var item = new Item(article, name, price);
                            items.Add(item);
                        }
                    }
                    else
                    {
                        throw new Exception("Line does not match pattern");
                    }
                }
                return items;
            }
        } 
    }
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
