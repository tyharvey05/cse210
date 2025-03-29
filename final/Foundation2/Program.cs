using System;

class Product
{
    private string _name;
    private double _price;
    private int _productId;

    public Product(string name, double price, int productId)
    {
        _name = name;
        _price = price;
        _productId = productId;
    }

    public string GetName() => _name;
    public double GetPrice() => _price;
    public int GetProductId() => _productId;
}

class Address
{
    private string _street;
    private string _city;
    private string _zipCode;

    public Address(string street, string city, string zipCode)
    {
        _street = street;
        _city = city;
        _zipCode = zipCode;
    }

    public string GetFullAddress() => $"{_street}, {_city}, {_zipCode}";
}

class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public string GetName() => _name;
    public Address GetAddress() => _address;
}

class Order
{
    private Customer _customer;
    private Product _product;
    private int _quantity;
    private double _totalPrice;

    public Order(Customer customer, Product product, int quantity)
    {
        _customer = customer;
        _product = product;
        _quantity = quantity;
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        _totalPrice = _product.GetPrice() * _quantity;
    }

    public string GetOrderDetails()
    {
        return $"Customer: {_customer.GetName()}\n" +
               $"Address: {_customer.GetAddress().GetFullAddress()}\n" +
               $"Product: {_product.GetName()}\n" +
               $"Product ID: {_product.GetProductId()}\n" +
               $"Price per Unit: ${_product.GetPrice():F2}\n" +
               $"Quantity: {_quantity}\n" +
               $"Total Price: ${_totalPrice:F2}";
    }
}

class Program
{
    static void Main()
    {
        Address address = new Address("123 Main St", "Springfield", "12345");
        Customer customer = new Customer("Alice", address);
        Product product = new Product("Laptop", 999.99, 101);
        Order order = new Order(customer, product, 2);

        Console.WriteLine(order.GetOrderDetails());
    }
}