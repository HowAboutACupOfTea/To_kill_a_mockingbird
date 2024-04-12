namespace WareHouseManager;

public interface IOrder
{
    public bool IsFilled();

    public bool CanFillOrder(IWarehouse warehouse);

    public void Fill(IWarehouse warehouse);
}