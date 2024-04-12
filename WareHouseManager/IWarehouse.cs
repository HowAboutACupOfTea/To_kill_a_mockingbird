namespace WareHouseManager;

public interface IWarehouse
{
    bool HasProduct(string product);

    int CurrentStock(string product);

    void AddStock(string product, int amount);

    void TakeStock(string product, int amount);
}
