using System;
using System.Collections.Generic;
using System.Linq;


public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ShoppingCartItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ShoppingCart
{
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public decimal TotalPrice(List<Product> products)
    {
        decimal totalPrice = 0;
        foreach (var item in Items)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product != null)
            {
                totalPrice += product.Price * item.Quantity;
            }
        }
        return totalPrice;
    }
}

public class Order
{
    public int OrderId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    public decimal TotalPrice { get; set; }
}

public class Program
{
    private static List<User> users = new List<User>();
    private static List<Product> products = new List<Product>();
    private static List<Order> orders = new List<Order>();
    private static ShoppingCart shoppingCart = new ShoppingCart();
    private static User currentUser = null;

    public static void Main(string[] args)
    {
        InitializeData();

        while (true)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. View Products");
            Console.WriteLine("4. Add to Cart");
            Console.WriteLine("5. View Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    ViewProducts();
                    break;
                case "4":
                    AddToCart();
                    break;
                case "5":
                    ViewCart();
                    break;
                case "6":
                    Checkout();
                    break;
                case "7":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    private static void InitializeData()
    {
        // Adding dummy data
        users.Add(new User { UserId = 1, Username = "user1", Password = "password1" });
        users.Add(new User { UserId = 2, Username = "user2", Password = "password2" });

        products.Add(new Product { ProductId = 1, Name = "Product 1", Price = 10 });
        products.Add(new Product { ProductId = 2, Name = "Product 2", Price = 20 });
        products.Add(new Product { ProductId = 3, Name = "Product 3", Price = 30 });
    }

    private static void Register()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (users.Any(u => u.Username == username))
        {
            Console.WriteLine("Username already exists!");
        }
        else
        {
            int userId = users.Count + 1;
            users.Add(new User { UserId = userId, Username = username, Password = password });
            Console.WriteLine("Registration successful!");
        }
    }

    private static void Login()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        currentUser = users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (currentUser != null)
        {
            Console.WriteLine($"Welcome, {currentUser.Username}!");
        }
        else
        {
            Console.WriteLine("Invalid username or password!");
        }
    }

    private static void ViewProducts()
    {
        Console.WriteLine("Products:");
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.ProductId}, Name: {product.Name}, Price: {product.Price}");
        }
    }

    private static void AddToCart()
    {
        if (currentUser == null)
        {
            Console.WriteLine("Please login first!");
            return;
        }

        Console.Write("Enter product ID to add to cart: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            Console.Write("Enter quantity: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
            {
                if (products.Any(p => p.ProductId == productId))
                {
                    shoppingCart.Items.Add(new ShoppingCartItem { ProductId = productId, Quantity = quantity });
                    Console.WriteLine("Product added to cart!");
                }
                else
                {
                    Console.WriteLine("Invalid product ID!");
                }
            }
            else
            {
                Console.WriteLine("Invalid quantity!");
            }
        }
        else
        {
            Console.WriteLine("Invalid product ID!");
        }
    }

    private static void ViewCart()
    {
        Console.WriteLine("Shopping Cart:");
        foreach (var item in shoppingCart.Items)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product != null)
            {
                Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Quantity: {item.Quantity}");
            }
        }
        Console.WriteLine($"Total Price: {shoppingCart.TotalPrice(products)}");
    }

    private static void Checkout()
    {
        if (currentUser == null)
        {
            Console.WriteLine("Please login first!");
            return;
        }

        if (shoppingCart.Items.Count == 0)
        {
            Console.WriteLine("Your cart is empty!");
            return;
        }

        decimal totalPrice = shoppingCart.TotalPrice(products);
        orders.Add(new Order { OrderId = orders.Count + 1, Items = shoppingCart.Items, TotalPrice = totalPrice });
        shoppingCart.Items.Clear();
        Console.WriteLine("Order placed successfully!");
    }
}
