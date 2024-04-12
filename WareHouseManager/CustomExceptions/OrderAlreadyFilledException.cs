namespace WareHouseManager.CustomExceptions;

public class OrderAlreadyFilledException :Exception
{
    public OrderAlreadyFilledException()
    {
    }

    public OrderAlreadyFilledException(string message) : base(message)
    {
    }

    public OrderAlreadyFilledException(string message, Exception inner) : base(message, inner)
    {
    }
}
