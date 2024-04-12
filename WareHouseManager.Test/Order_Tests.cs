using Moq;

namespace WareHouseManager.Test;

internal class Order_Tests
{
    [TestCase("")]
    [TestCase(" ")]
    public void Empty_Product_Name_Throws_Exception(string productName)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Order(productName, 0));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Can_Not_Order_Less_Than_One(int amountToOrder)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Order("myProduct", amountToOrder));
    }

    [TestCase("myProduct", 1)]
    public void A_New_Order_Is_Not_Filled(string productToOrder, int amountToOrder)
    {
        // Arrange
        Order order = new(productToOrder, amountToOrder);

        // Act
        bool actualIsFilled = order.IsFilled();

        // Assert
        Assert.That(actualIsFilled, Is.False);
    }

    [TestCase("myProduct", 1)]
    public void An_Order_Can_Be_Filled(string productToOrder, int amountToOrder)
    {
        // Arrange
        Order order = new(productToOrder, amountToOrder);
        Mock<IWarehouse> warehouseMock = new();
        warehouseMock.Setup(w => w.HasProduct(It.IsAny<string>())).Returns(true);
        warehouseMock.Setup(w => w.CurrentStock(It.IsAny<string>())).Returns(amountToOrder);

        // Act
        bool canOrderBeFilled = order.CanFillOrder(warehouseMock.Object);

        // Assert
        Assert.That(canOrderBeFilled);
        warehouseMock.Verify(w => w.HasProduct(It.IsAny<string>()), Times.Once());
        warehouseMock.Verify(w => w.CurrentStock(It.IsAny<string>()), Times.Once());
        warehouseMock.VerifyNoOtherCalls();
    }

    [TestCase("myProduct", 1)]
    public void An_Order_Needs_A_Stocked_Product_To_Be_Fillable(string productToOrder, int amountToOrder)
    {
        // Arrange
        Order order = new(productToOrder, amountToOrder);
        Mock<IWarehouse> warehouseMock = new();
        warehouseMock.Setup(w => w.HasProduct(It.IsAny<string>())).Returns(false);
        warehouseMock.Setup(w => w.CurrentStock(It.IsAny<string>())).Returns(amountToOrder);

        // Act
        bool canOrderBeFilled = order.CanFillOrder(warehouseMock.Object);

        // Assert
        Assert.That(canOrderBeFilled, Is.False);
        warehouseMock.Verify(w => w.HasProduct(It.IsAny<string>()), Times.Once());
        warehouseMock.Verify(w => w.CurrentStock(It.IsAny<string>()), Times.Once());
        warehouseMock.VerifyNoOtherCalls();
    }

    [TestCase("myProduct", 1)]
    public void Ordering_More_Stock_Than_Stored_Is_Not_Possible(string productToOrder, int amountToOrder)
    {
        // Arrange
        Order order = new(productToOrder, amountToOrder);
        Mock<IWarehouse> warehouseMock = new();
        warehouseMock.Setup(w => w.HasProduct(It.IsAny<string>())).Returns(true);
        warehouseMock.Setup(w => w.CurrentStock(It.IsAny<string>())).Returns(0);

        // Act
        bool canOrderBeFilled = order.CanFillOrder(warehouseMock.Object);

        // Assert
        Assert.That(canOrderBeFilled, Is.False);
        warehouseMock.Verify(w => w.HasProduct(It.IsAny<string>()), Times.Once());
        warehouseMock.Verify(w => w.CurrentStock(It.IsAny<string>()), Times.Once());
        warehouseMock.VerifyNoOtherCalls();
    }

    [TestCase()]
    public void Can_Not_Fill_Already_Filled_Order()
    {
        // Arrange
        Mock<IWarehouse> warehouseMock = new();
        warehouseMock.Setup(w => w.TakeStock("ab", 1));
        Order order = new("myProduct", 1);
        order.Fill(warehouseMock.Object);

        // Act
        // Assert
        Assert.Throws<OrderAlreadyFilledException>(() => order.Fill(warehouseMock.Object));
        warehouseMock.Verify(w => w.TakeStock(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        warehouseMock.VerifyNoOtherCalls();
    }

    [TestCase("myProduct", 1)]
    public void Fill_Order(string productToOrder, int amountToOrder)
    {
        // Arrange
        Order order = new(productToOrder, amountToOrder);
        Mock<IWarehouse> warehouseMock = new();
        warehouseMock.Setup(w => w.HasProduct(It.IsAny<string>())).Returns(true);
        warehouseMock.Setup(w => w.CurrentStock(It.IsAny<string>())).Returns(amountToOrder);

        // Act
        order.Fill(warehouseMock.Object);

        // Assert
        Assert.That(order.IsFilled());
        warehouseMock.Verify(w => w.TakeStock(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        warehouseMock.VerifyNoOtherCalls();
    }
}