namespace WareHouseManager.Test;

internal class Warehouse_Tests
{
    private Warehouse _warehouse;
    private readonly Dictionary<string, int> _stubOfAvailableStock = new()
    {
        { "Apple", 0 },
        { "Banana", 1 },
        { "Cherry", 9999 }
    };

    [SetUp]
    public void SetUp()
    {
        _warehouse = new();
    }

    [TestCase("")]
    [TestCase(" ")]
    public void Empty_Product_Name_Throws_Exception(string productName)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => _warehouse.HasProduct(productName));
        Assert.Throws<ArgumentException>(() => _warehouse.CurrentStock(productName));
        Assert.Throws<ArgumentException>(() => _warehouse.AddStock(productName, 0));
        Assert.Throws<ArgumentException>(() => _warehouse.TakeStock(productName, 0));
    }

    [TestCase("Apple", true)]
    [TestCase("Banana", true)]
    [TestCase("Cherry", true)]
    [TestCase("Microchips", false)]
    [TestCase("Blankets", false)]
    public void Product_Is_In_The_Warehouse(string productName, bool expectedResult)
    {
        // Arrange
        _warehouse._availableStock = _stubOfAvailableStock;

        // Act
        // Assert
        Assert.That(_warehouse.HasProduct(productName), Is.EqualTo(expectedResult));
    }

    [TestCase("Apple")]
    public void Product_Without_Stock_Can_Be_In_The_Warehouse(string productName)
    {
        // Arrange
        _warehouse._availableStock = _stubOfAvailableStock;

        // Act
        // Assert
        Assert.That(_warehouse.HasProduct(productName));
    }

    [TestCase("IAmNotStored")]
    public void Can_Not_Get_Stock_Of_Not_Stored_Product(string productName)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<NoSuchProductException>(() => _warehouse.CurrentStock(productName));
    }

    [TestCase("Apple", 0)]
    [TestCase("Banana", 1)]
    [TestCase("Cherry", 9999)]
    public void Get_Current_Stock_Of_Product(string productName, int expectedStock)
    {
        // Arrange
        _warehouse._availableStock = _stubOfAvailableStock;

        // Act
        int actualStock = _warehouse.CurrentStock(productName);

        // Assert
        Assert.That(actualStock, Is.EqualTo(expectedStock));
    }

    [Test]
    public void Can_Not_Add_Negative_Amount_Of_Stock_To_Warehouse()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _warehouse.AddStock("MyProduct", -1));
    }

    [TestCase("MyProduct", 0)]
    [TestCase("MyProduct", 1)]
    [TestCase("MyProduct", 9999)]
    public void Add_Product_To_Warehouse(string productName, int startingStock)
    {
        // Arrange
        // Act
        _warehouse.AddStock(productName, startingStock);

        // Assert
        Assert.That(_warehouse._availableStock.ContainsKey(productName));
    }

    [TestCase("Apple", 0)]
    [TestCase("Apple", 1)]
    [TestCase("Apple", 9999)]
    public void Add_Certain_Amount_Of_Stock_To_Warehouse(string productName, int expectedStock)
    {
        // Arrange
        // Act
        _warehouse.AddStock(productName, expectedStock);
        int actualStock = _warehouse._availableStock[productName];

        // Assert
        Assert.That(actualStock, Is.EqualTo(expectedStock));
    }

    [TestCase("IAmNotStored")]
    public void Can_Not_Take_Stock_Of_Not_Stored_Product(string productName)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<NoSuchProductException>(() => _warehouse.TakeStock(productName, 1));
    }

    [TestCase("Apple", 1)]
    [TestCase("Banana", 2)]
    [TestCase("Cherry", 10000)]
    public void Can_Not_Take_More_Than_The_Stored_Amount_Of_Stock(string productName, int stockToTake)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<NoSuchProductException>(() => _warehouse.TakeStock(productName, 1));
    }

    [TestCase("Apple")]
    [TestCase("Banana")]
    [TestCase("Cherry")]
    public void Can_Not_Take_Negative_Amount_Of_Stock(string productName)
    {
        // Arrange
        _warehouse._availableStock = _stubOfAvailableStock;

        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _warehouse.TakeStock(productName, -1));
    }

    [TestCase("Apple", 0, 0)]
    [TestCase("Banana", 1, 0)]
    [TestCase("Cherry", 999, 9000)]
    public void Take_Stock_From_Warehouse(string productName, int stockToTake, int remainingStock)
    {
        // Arrange
        _warehouse._availableStock = _stubOfAvailableStock;

        // Act
        _warehouse.TakeStock(productName, stockToTake);
        int actualStock = _warehouse._availableStock[productName];

        // Assert
        Assert.That(actualStock, Is.EqualTo(remainingStock));
    }
}
