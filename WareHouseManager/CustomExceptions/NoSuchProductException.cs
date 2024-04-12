namespace WareHouseManager.CustomExceptions;

public class NoSuchProductException : Exception
{
    public NoSuchProductException()
    {
    }

    public NoSuchProductException(string message) : base(message)
    {
    }

    public NoSuchProductException(string message, Exception inner) : base(message, inner)
    {
    }
}
