using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Клас Товар
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }

    public Product(string name, decimal price, string description, string category)
    {
        Name = name;
        Price = price;
        Description = description;
        Category = category;
    }
}

// Клас Користувач
class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Order> PurchaseHistory { get; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        PurchaseHistory = new List<Order>();
    }
}

// Клас Замовлення
class Order
{
    public List<Product> Products { get; }
    public Dictionary<Product, int> ProductQuantity { get; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }

    public Order()
    {
        Products = new List<Product>();
        ProductQuantity = new Dictionary<Product, int>();
        TotalPrice = 0;
        Status = "Pending";
    }
}

// Інтерфейс для пошуку товарів
interface ISearchable
{
    List<Product> SearchByPriceRange(decimal minPrice, decimal maxPrice);
    List<Product> SearchByCategory(string category);
}

// Клас Магазин
class Store : ISearchable
{
    public List<Product> Products { get; }
    public List<User> Users { get; }
    public List<Order> Orders { get; }

    public Store()
    {
        Products = new List<Product>();
        Users = new List<User>();
        Orders = new List<Order>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void RegisterUser(User user)
    {
        Users.Add(user);
    }

    public void CreateOrder(User user, List<Product> selectedProducts)
    {
        var order = new Order();
        foreach (var product in selectedProducts)
        {
            order.Products.Add(product);
            if (order.ProductQuantity.ContainsKey(product))
            {
                order.ProductQuantity[product]++;
            }
            else
            {
                order.ProductQuantity[product] = 1;
            }
            order.TotalPrice += product.Price;
        }
        Orders.Add(order);
        user.PurchaseHistory.Add(order);
    }

    public List<Product> SearchByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}

class Program
{
    static void Main()
    {
        var store = new Store();

        var user1 = new User("user1", "password1");
        var user2 = new User("user2", "password2");

        store.RegisterUser(user1);
        store.RegisterUser(user2);

        var product1 = new Product("Product 1", 10.99M, "Description 1", "Category A");
        var product2 = new Product("Product 2", 15.99M, "Description 2", "Category B");
        var product3 = new Product("Product 3", 5.99M, "Description 3", "Category A");

        store.AddProduct(product1);
        store.AddProduct(product2);
        store.AddProduct(product3);

        var selectedProducts = new List<Product> { product1, product2, product3 };
        store.CreateOrder(user1, selectedProducts);

        Console.WriteLine("Purchase history for user1:");
        foreach (var order in user1.PurchaseHistory)
        {
            Console.WriteLine($"Order status: {order.Status}, Total price: {order.TotalPrice:C2}");
            foreach (var product in order.Products)
            {
                Console.WriteLine($"Product: {product.Name}, Quantity: {order.ProductQuantity[product]}, Price: {product.Price:C2}");
            }
        }

        Console.WriteLine("\nProducts in Category A:");
        var categoryAProducts = store.SearchByCategory("Category A");
        foreach (var product in categoryAProducts)
        {
            Console.WriteLine($"Product: {product.Name}, Category: {product.Category}");
        }
            
        Console.ReadLine();
    }
}
