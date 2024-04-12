using System.Runtime.CompilerServices;
using WareHouseManager.CustomExceptions;

[assembly: InternalsVisibleTo("WareHouseManager.Test")]

namespace WareHouseManager;

public class Order : IOrder
{
    private bool _isFilled;
    private readonly string _product;
    private readonly int _amount;

    public Order(string product, int amount)
    {
        ThrowWhenProductNameIsInvalid(product);
        ThrowWhenAmountIsTooLow(amount);
        _isFilled = false;
        _product = product;
        _amount = amount;
    }

    public bool IsFilled()
    {
        return _isFilled;
    }

    public bool CanFillOrder(IWarehouse warehouse)
    {
        bool isProductInWarehouse = warehouse.HasProduct(_product);
        bool isStockSufficient = warehouse.CurrentStock(_product) >= _amount;
        return isProductInWarehouse && isStockSufficient;
    }

    public void Fill(IWarehouse warehouse)
    {
        ThrowWhenOrderIsAlreadyFilled();

        try
        {
            warehouse.TakeStock(_product, _amount);
        }
        catch (Exception e)
        {
            throw new Exception($"A problem ocurred in the {nameof(IWarehouse)} implementation.", e);
        }

        _isFilled = true;
    }

    private static void ThrowWhenProductNameIsInvalid(string product)
    {
        if (string.IsNullOrWhiteSpace(product))
        {
            throw new ArgumentException($"The product name {product} must not be null or empty.");
        }
    }

    private static void ThrowWhenAmountIsTooLow(int amount)
    {
        int lowerLimit = 1;

        if (amount < lowerLimit)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"The taken amount must be more than {lowerLimit}.");
        }
    }

    private void ThrowWhenOrderIsAlreadyFilled()
    {
        if (IsFilled())
        {
            throw new OrderAlreadyFilledException();
        }
    }
}