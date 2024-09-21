// PRogram.cs 
//Design Patterns
// Assignment 1 
//Written by Atem Ako and Kasra Bina 
//Last Modified : 12th September 2024
using System;
using System.Collections.Generic;

namespace RetailApp
{
    public class RetailStore
    {
        private List<Customer> customers = new List<Customer>();
        private double totalRevenue;
        private double toolRevenue;
        private double groceryRevenue;

        public void AddCustomer(Customer customer)
        {
            customers.Add(customer);
        }

        public void AddToolRevenue(double amount)
        {
            toolRevenue += amount * 1.14; // Applying GST
            totalRevenue += amount * 1.14;
        }

        public void AddGroceryRevenue(double amount)
        {
            groceryRevenue += amount;
            totalRevenue += amount;
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\nRetail Store Statistics:");
            Console.WriteLine($"Total Revenue: ${totalRevenue:f2}");
            Console.WriteLine($"Total Tool Revenue: ${toolRevenue:f2} ({(toolRevenue / totalRevenue) * 100:f2}%)");
            Console.WriteLine($"Total Grocery Revenue: ${groceryRevenue:f2} ({(groceryRevenue / totalRevenue) * 100:f2}%)");
            Console.WriteLine($"Average Spending Per Customer: ${(totalRevenue / customers.Count):f2}");
        }

        public void DisplayCustomerPurchases()
        {
            foreach (var customer in customers)
            {
                customer.DisplayCart();
            }
        }
    }

    public class ShoppingCart
    {
        private List<ITool> tools;
        private List<IGrocery> groceries;

        public ShoppingCart()
        {
            tools = new List<ITool>();
            groceries = new List<IGrocery>();
        }

        public void AddTool(ITool tool)
        {
            tools.Add(tool);
        }

        public void AddGrocery(IGrocery grocery)
        {
            groceries.Add(grocery);
        }

        public double GetTotalCost()
        {
            double totalCost = 0;
            foreach (var tool in tools)
            {
                totalCost += tool.GetCost() * 1.14; // Applying GST for tools
            }
            foreach (var grocery in groceries)
            {
                totalCost += grocery.GetCost();
            }
            return totalCost;
        }

        public void DisplayCart()
        {
            Console.WriteLine("Shopping Cart Items:");
            foreach (var tool in tools)
            {
                tool.Display();
            }
            foreach (var grocery in groceries)
            {
                grocery.Display();
            }
            Console.WriteLine($"Total cost: ${GetTotalCost():f2}\n");
        }

        public double GetToolCost()
        {
            double totalToolCost = 0;
            foreach (var tool in tools)
            {
                totalToolCost += tool.GetCost() * 1.14; // Applying GST for tools
            }
            return totalToolCost;
        }

        public double GetGroceryCost()
        {
            double totalGroceryCost = 0;
            foreach (var grocery in groceries)
            {
                totalGroceryCost += grocery.GetCost();
            }
            return totalGroceryCost;
        }
    }

    public class Customer
    {
        private string name;
        private double cash;
        private RetailStore store;
        private ShoppingCart cart;

        public Customer(string name, double cash, RetailStore store)
        {
            this.name = name;
            this.cash = cash;
            this.store = store;
            this.cart = new ShoppingCart();
            store.AddCustomer(this);
        }

        public void AddTool(ITool tool)
        {
            if (cash >= tool.GetCost())
            {
                cart.AddTool(tool);
                cash -= tool.GetCost();
                store.AddToolRevenue(tool.GetCost());
            }
            else
            {
                Console.WriteLine($"{name} does not have enough cash to buy the tool.");
            }
        }

        public void AddGrocery(IGrocery grocery)
        {
            if (cash >= grocery.GetCost())
            {
                cart.AddGrocery(grocery);
                cash -= grocery.GetCost();
                store.AddGroceryRevenue(grocery.GetCost());
            }
            else
            {
                Console.WriteLine($"{name} does not have enough cash to buy the grocery.");
            }
        }

        public void DisplayCart()
        {
            Console.WriteLine($"{name}'s Shopping Cart:");
            cart.DisplayCart();
        }

        public double GetTotalSpent()
        {
            return cart.GetTotalCost();
        }

        public double GetToolSpent()
        {
            return cart.GetToolCost();
        }

        public double GetGrocerySpent()
        {
            return cart.GetGroceryCost();
        }
    }

    public interface IGrocery
    {
        double GetCost();
        void Display();
    }

    public class Apple : IGrocery
    {
        private string name;
        private double cost;
        private double calories;

        public Apple(string name, double cost, double calories)
        {
            this.name = name;
            this.cost = cost;
            this.calories = calories;
        }

        public double GetCost()
        {
            return cost;
        }

        public void Display()
        {
            Console.WriteLine($"{name} is an apple. It has {calories} calories and costs ${cost:f2}.");
        }
    }

    public class Orange : IGrocery
    {
        private string name;
        private double cost;
        private double calories;

        public Orange(string name, double cost, double calories)
        {
            this.name = name;
            this.cost = cost;
            this.calories = calories;
        }

        public double GetCost()
        {
            return cost;
        }

        public void Display()
        {
            Console.WriteLine($"{name} is an orange. It has {calories} calories and costs ${cost:f2}.");
        }
    }

    public class GroceryBag
    {
        private string name;
        private double totalCost;
        private List<IGrocery> items;
        private int numItems;

        public GroceryBag(string name)
        {
            this.name = name;
            totalCost = 0.0;
            items = new List<IGrocery>();
            numItems = 0;
        }

        public bool AddGrocery(IGrocery grocery)
        {
            if (numItems < 4)
            {
                items.Add(grocery);
                numItems++;
                totalCost += grocery.GetCost();
                return true;
            }

            return false;
        }

        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine($"{name} is a grocery bag with the following items:");

            foreach (var item in items)
            {
                item.Display();
            }

            Console.WriteLine($"The total cost of these items is ${totalCost:f2}\n");
        }
    }

    public interface ITool
    {
        double GetCost();
        void Display();
    }

    public class Hammer : ITool
    {
        private string name;
        private double cost;

        public Hammer(string name, double cost)
        {
            this.name = name;
            this.cost = cost;
        }

        public double GetCost()
        {
            return cost;
        }

        public void Display()
        {
            Console.WriteLine($"{name} is a hammer and costs ${cost:f2}.");
        }
    }

    public class Screwdriver : ITool
    {
        private string name;
        private double cost;

        public Screwdriver(string name, double cost)
        {
            this.name = name;
            this.cost = cost;
        }

        public double GetCost()
        {
            return cost;
        }

        public void Display()
        {
            Console.WriteLine($"{name} is a screwdriver and costs ${cost:f2}.");
        }
    }

    public class Toolbox
    {
        private string name;
        private double totalCost;
        private List<ITool> items;
        private int numItems;

        public Toolbox(string name)
        {
            this.name = name;
            totalCost = 0.0;
            items = new List<ITool>();
            numItems = 0;
        }

        public bool AddTool(ITool tool)
        {
            if (numItems < 4)
            {
                items.Add(tool);
                numItems++;
                totalCost += tool.GetCost();
                return true;
            }

            return false;
        }

        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine($"{name} is a toolbox with the following items:");

            foreach (var item in items)
            {
                item.Display();
            }

            Console.WriteLine($"The total cost of these items is ${totalCost:f2}\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RetailStore store = new RetailStore();

            Customer customer1 = new Customer("Alice", 100, store);
            Customer customer2 = new Customer("Bob", 50, store);
            Customer customer3 = new Customer("Charlie", 150, store);

            customer1.AddTool(new Hammer("Hammer", 20.0));
            customer1.AddGrocery(new Apple("Apple", 2.5, 95));

            customer2.AddTool(new Screwdriver("Screwdriver", 15.0));
            customer2.AddGrocery(new Orange("Orange", 3.0, 85));

            customer3.AddTool(new Hammer("Big Hammer", 35.0));
            customer3.AddTool(new Screwdriver("Small Screwdriver", 10.0));
            customer3.AddGrocery(new Apple("Green Apple", 2.8, 100));
            customer3.AddGrocery(new Orange("Navel Orange", 3.2, 90));

            store.DisplayCustomerPurchases();
            store.DisplayStatistics();
        }
    }
}
