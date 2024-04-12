using System.Runtime.CompilerServices;
using WareHouseManager.CustomExceptions;

[assembly: InternalsVisibleTo("WareHouseManager.Test")]

namespace WareHouseManager;

public class Warehouse : IWarehouse
{
    internal Dictionary<string, int> _availableStock;

    public Warehouse()
    {
        _availableStock = new();
    }
 
    public Dictionary<string, int> GetWarehouseStock()
    {
        return _availableStock;
    }

    public bool HasProduct(string product)
    {
        ThrowWhenProductNameIsInvalid(product);
        return _availableStock.ContainsKey(product);
    }

    public int CurrentStock(string product)
    {
        ThrowWhenProductNameIsInvalid(product);
        ThrowWhenProductDoesNotExist(product);
        return _availableStock[product];
    }

    public void AddStock(string product, int amount)
    {
        ThrowWhenProductNameIsInvalid(product);
        ThrowWhenAmountIsNegative(amount);

        if (!HasProduct(product))
        {
            _availableStock.Add(product, 0);
        }
        
        _availableStock[product] += amount;
    }

    public void TakeStock(string product, int amount)
    {
        ThrowWhenProductNameIsInvalid(product);
        ThrowWhenProductDoesNotExist(product);
        ThrowWhenStockIsNotSufficient(product, amount);
        ThrowWhenAmountIsNegative(amount);
        _availableStock[product] -= amount;
    }

    private static void ThrowWhenProductNameIsInvalid(string product)
    {
        if (string.IsNullOrWhiteSpace(product))
        {
            throw new ArgumentException($"The product name {product} must not be null or empty.");
        }
    }

    private void ThrowWhenProductDoesNotExist(string product)
    {
        if (!HasProduct(product))
        {
            throw new NoSuchProductException($"No product with the name {product} exists in the {nameof(Warehouse)}.");
        }
    }

    private void ThrowWhenStockIsNotSufficient(string product, int amount)
    {
        if (amount > CurrentStock(product))
        {
            throw new InsufficientStockException($"The product {product} does not have enough stock.");
        }
    }

    private static void ThrowWhenAmountIsNegative(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"The {nameof(amount)} must not be negative.");
        }
    }
}
