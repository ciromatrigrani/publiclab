namespace gRPCDiscountAPI.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string productName, Exception inner = null) : base(String.Format("Not found: {0}", productName), inner)
    { }
}
