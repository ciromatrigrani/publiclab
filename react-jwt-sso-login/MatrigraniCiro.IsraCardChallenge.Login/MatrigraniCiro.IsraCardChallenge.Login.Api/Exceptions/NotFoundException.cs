namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    { }

    public NotFoundException(Guid guid, Exception inner = null) : base(
        $"The identifier '{guid}' was not found. Have you sure it is the correct identifier? Please check it and try again.", inner)
    { }
}

